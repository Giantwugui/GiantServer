using Giant.Msg;
using Giant.Net;

namespace Server.App
{
    partial class Service
    {
        private void Test(Session session, IMessage message)
        {
            GC_TEST response = message as GC_TEST;
            response.Result = ErrorCode.ERR_Success;

            session.Send(response);
        }
    }
}
