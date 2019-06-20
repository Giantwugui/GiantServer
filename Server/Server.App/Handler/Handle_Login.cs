using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.App
{
    [MessageHandler(AppType.Gate)]
    public class Handle_Login : MRpcHandler<CR_Login, RC_Login>
    {
        public override async void Run(Session session, CR_Login request, Action<RC_Login> reply)
        {
            RC_Login response = new RC_Login() { RpcId = request.RpcId };

            try
            {
                var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
                AccountInfo account = await query.Task();
                if (account == null)
                {
                    response.Error = ErrorCode.RpcFail;
                    reply(response);
                    return;
                }

                Logger.Warn($"player login {request.Account}");

                response.Error = ErrorCode.Success;
                reply(response);
            }
            catch (Exception ex)
            {
                response.Error = ErrorCode.RpcFail;
                reply(response);
                Logger.Error(ex);
            }
        }
    }

    [MessageHandler(AppType.Gate)]
    public class Handle_Ping : MRpcHandler<CR_Ping, RC_Ping>
    {
        public override void Run(Session session, CR_Ping request, Action<RC_Ping> reply)
        {
            RC_Ping response = new RC_Ping() { RpcId = request.RpcId };

            try
            {
                response.Error = ErrorCode.Success;
                reply(response);
            }
            catch (Exception ex)
            {
                response.Error = ErrorCode.RpcFail;
                reply(response);
                Logger.Error(ex);
            }
        }
    }
}
