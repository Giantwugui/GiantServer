using Giant.Core;
using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using Server.Frame;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Account
{
    [MessageHandler]
    class Handle_CA_Login : RpcMHandler<Msg_CA_Login, Msg_AC_Login>
    {
        public override async Task Run(Session session, Msg_CA_Login request, Msg_AC_Login response)
        {
            var query = new MongoDBQuery<AccountInfo>(DBName.Account, x => x.Account == request.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                string securityPwd = MD5Helper.Encrypt(request.Password);
                account = new AccountInfo() { Account = request.Account, Password = securityPwd, RegistTime = TimeHelper.NowString };
                var insertQuery = new MongoDBInsert<AccountInfo>(DBName.Account, account);
                await insertQuery.Task();
            }
            else
            {
                if (account.Servers != null && account.Servers.Count > 0)
                {
                    response.Servers.AddRange(account.Servers);
                }
            }

            //更新最后登录时间
            account.LastLoginTime = TimeHelper.NowString;
            await account.UpdateTask();

            response.Error = ErrorCode.Success;

            Logger.Debug($"user login {request.Account}");
        }
    }

    [MessageHandler]
    class ClientHandle_Login_Zone : RpcMHandler<Msg_CA_LoginZone, Msg_AC_LoginZone>
    {
        public override async Task Run(Session session, Msg_CA_LoginZone request, Msg_AC_LoginZone response)
        {
            var query = new MongoDBQuery<AccountInfo>(DBName.Account, x => x.Account == request.Account);
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
                if (account.Servers.Contains(request.Zone))
                {
                    account.Servers.Add(request.Zone);
                    await account.UpdateTask();
                }
            }

            GateInfo gateInfo = GateInfoManager.Instance.GetGateWithBalance(request.Zone);
            if (gateInfo == null)
            {
                Logger.Error($"User {request.Account} request server {request.Zone}, have not exist !");
                response.Error = ErrorCode.HaveNotFindServer;
                return;
            }

            response.IP = gateInfo.IP;
            response.Port = gateInfo.Port;
            response.Key = gateInfo.ClientCount + 1;

            Logger.Debug($"user login {session.RemoteIPEndPoint} {request.Account}");
        }
    }
}
