using Giant.Core;
using Giant.DB.MongoDB;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using Giant.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.App
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

            Log.Debug($"user login {request.Account}");
        }
    }

    [MessageHandler]
    class Handle_Login_Zone : RpcMHandler<Msg_CA_LoginZone, Msg_AC_LoginZone>
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
                if (!account.Servers.Contains(request.Zone))
                {
                    account.Servers.Add(request.Zone);
                    await account.UpdateTask();
                }
            }

            GateInfo gateInfo = GateInfoComponent.Instance.GetGateWithBalance(request.Zone);
            if (gateInfo == null)
            {
                Log.Error($"User {request.Account} request server {request.Zone}, have not exist !");
                response.Error = ErrorCode.HaveNotFindServer;
                return;
            }

            response.Address = gateInfo.Address;
            response.Token = gateInfo.ClientCount + 1;

            Msg_AG_ClientLogin loginMsg = new Msg_AG_ClientLogin()
            {
                Account = request.Account,
                Token = response.Token,
            };

            var server = NetProxyComponent.Instance.GetBackend(AppType.Gate, gateInfo.AppId, gateInfo.SubId);
            server.Session.Notify(loginMsg);

            Log.Debug($"user login {session.RemoteIPEndPoint} {request.Account}");
        }
    }
}
