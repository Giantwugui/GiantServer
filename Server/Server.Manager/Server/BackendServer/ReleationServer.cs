using Giant.Net;
using Giant.Share;
using Server.Frame;

namespace Server.Manager
{
    public class ReleationServer : BackendServer
    {
        public ReleationServer(BackendServerManager manager, AppType appType, int appId, int subId, Session session)
            : base(manager, appType, appId, subId, session)
        { }
    }
}
