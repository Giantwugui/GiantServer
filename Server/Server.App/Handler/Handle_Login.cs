using Giant.Log;
using Giant.Msg;
using Giant.Net;

namespace Server.App
{
    [MessageHandler(ServerType.Gate)]
    public class Handle_Login : MsgHandler<CR_LOGIN>
    {
        public override void Handle(Session session, IMessage message)
        {
            CR_LOGIN msg = message as CR_LOGIN;
            RC_LOGIN response = new RC_LOGIN();

            if (msg == null)
            {
                response.Error = ErrorCode.ERR_RpcFail;
                return;
            }

            Logger.Warn($"player login {msg.Account}");

            response.RpcId = msg.RpcId;
            response.Error = ErrorCode.ERR_Success;
            session.Send(response);
        }
    }
}
