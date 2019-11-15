using Giant.Net;
using Giant.Share;
using Server.Frame;

namespace Server.Manager
{
    public class ZoneServer : BackendServer
    {
        public ZoneServer(BackendServerManager manager, AppType appType, int appId, int subId, Session session)
            : base(manager, appType, appId, subId, session)
        { }
    }
}
