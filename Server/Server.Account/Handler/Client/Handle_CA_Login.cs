using System.Threading.Tasks;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using Giant.Log;
using Giant.DB.MongoDB;
using System.Collections.Generic;
using Giant.Core;

namespace Server.Account
{
    [MessageHandler(AppType.Account)]
    class Handle_CA_Login : RpcMHandler<Msg_CA_Login, Msg_AC_Login>
    {
        public override async Task Run(Session session, Msg_CA_Login request, Msg_AC_Login response)
        {
            var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                string securityPwd = MD5Helper.Encrypt(request.Password);
                account = new AccountInfo() { Account = request.Account, Password = securityPwd, RegistTime = TimeHelper.NowString };
                var insertQuery = new MongoDBInsert<AccountInfo>("Account", account);
                await insertQuery.Task();
            }
            else
            {
                if (account.Servers != null && account.Servers.Count > 0)
                {
                    response.Zones.AddRange(account.Servers);
                }
            }

            //更新最后登录时间
            account.LastLoginTime = TimeHelper.NowString;
            await account.UpdateTask();

            response.Error = ErrorCode.Success;

            Logger.Debug($"user login {request.Account}");
        }
    }

    [MessageHandler(AppType.Account)]
    class ClientHandle_Login_Zone : RpcMHandler<Msg_CA_LoginServers, Msg_AC_LoginServers>
    {
        public override async Task Run(Session session, Msg_CA_LoginServers request, Msg_AC_LoginServers response)
        {
            var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                response.Error = ErrorCode.HaveNotFindAccount;
                return;
            }
            else
            {
                if (account.Servers == null)
                {
                    account.Servers = new List<int>() { request.Zone };
                }
                if(account.Servers.Contains(request.Zone))
                {
                    account.Servers.Add(request.Zone);
                    await account.UpdateTask();
                }
            }

            response.Error = ErrorCode.Success;

            Logger.Debug($"user login {session.RemoteIPEndPoint} {request.Account}");
        }
    }
}
