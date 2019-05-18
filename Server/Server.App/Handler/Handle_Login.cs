using Giant.Msg;
using Giant.Net;

namespace Server.App
{
    [MessageHandler(ServerType.Gate)]
    public class Handle_Login : MsgHandler<CG_LOGIN>
    {
        public override void Handle(Session session, IMessage message)
        {
            GC_LOGIN response = message as GC_LOGIN;
            response.Error = ErrorCode.ERR_Success;

            session.Send(response);
        }
    }
}
