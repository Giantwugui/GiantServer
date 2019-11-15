using Giant.Net;
using Giant.Share;
using Server.Frame;

namespace Server.Manager
{
    public class ServerFactory : BaseServerFactory
    {
        public ServerFactory(BaseAppService service) : base(service)
        {
        }

        public override BackendServer CreateBackendServer(Session session, AppType appType, int appId, int subId)
        {
            BackendServerManager manager = NetProxyManager.GetBackendServiceManager(appType);
            switch (appType)
            {
                case AppType.Gate:
                    return new GateServer(manager, appType, appId, subId, session);
                case AppType.Map:
                    return new ZoneServer(manager, appType, appId, subId, session);
                case AppType.Relation:
                    return new ReleationServer(manager, appType, appId, subId, session);
                default:
                    return base.CreateBackendServer(session, appType, appId, subId);
            }
        }
    }
}
