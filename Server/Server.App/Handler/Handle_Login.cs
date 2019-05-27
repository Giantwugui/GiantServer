using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using System;

namespace Server.App
{
    [MessageHandler(AppyType.Gate)]
    public class Handle_Login : MRpcHandler<CR_LOGIN, RC_LOGIN>
    {
        public override async void Run(Session session, CR_LOGIN request, Action<RC_LOGIN> reply)
        {
            RC_LOGIN response = new RC_LOGIN() { RpcId = request.RpcId };

            try
            {
                var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
                AccountInfo account = await query.Task();
                if (account == null)
                {
                    response.Error = ErrorCode.ERR_RpcFail;
                    reply(response);
                    return;
                }

                Logger.Warn($"player login {request.Account}");

                response.Error = ErrorCode.ERR_Success;
                reply(response);
            }
            catch (Exception ex)
            {
                response.Error = ErrorCode.ERR_RpcFail;
                reply(response);
                Logger.Error(ex);
            }
        }
    }
}
