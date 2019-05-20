using System;
using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;

namespace Server.App
{
    [MessageHandler(ServerType.Gate)]
    public class Handle_Login : MRpcHandler<CR_LOGIN, RC_LOGIN>
    {
        public override async void Run(Session session, IRequest message, Action<RC_LOGIN> reply)
        {
            CR_LOGIN msg = message as CR_LOGIN;
            RC_LOGIN response = new RC_LOGIN();
            response.RpcId = msg.RpcId;

            if (msg == null)
            {
                response.Error = ErrorCode.ERR_RpcFail;
                reply(response);
                return;
            }

            var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == msg.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                response.Error = ErrorCode.ERR_RpcFail;
                reply(response);
                return;
            }

            Logger.Warn($"player login {msg.Account}");

            response.Error = ErrorCode.ERR_Success;
            reply(response);
        }
    }
}
