using Giant.Msg;
using Giant.Net;

namespace Server.App
{
    [MessageHandler(ServerType.Gate)]
    public class Handle_Test : MsgHandler<GC_TEST>
    {
        public override void Handle(Session session, IMessage message)
        {
            GC_TEST response = message as GC_TEST;
            response.Error = ErrorCode.ERR_Success;

            session.Send(response);
        }
    }
}
