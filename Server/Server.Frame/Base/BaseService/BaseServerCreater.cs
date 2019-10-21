using Giant.Data;
using Giant.Net;
using Giant.Share;

namespace Server.Frame
{
    public class BaseServerCreater
    {
        public NetProxyManager NetProxyManager { get; private set; }

        public BaseServerCreater(NetProxyManager manager)
        {
            NetProxyManager = manager;
        }

        public virtual BackendService CreateBackendServer(Session session, AppType appType, int appId, int subId)
        {
            BackendServiceManager manager = NetProxyManager.GetBackendServiceManager(appType);
            BackendService backend = new BackendService(manager, appType, appId, subId, session);
            return backend;
        }

        public virtual FrontendService CreateFrontendServer(AppConfig config)
        {
            var manager = NetProxyManager.GetFrontendServiceManager(config.AppType);
            FrontendService frontend = new FrontendService(manager, config);
            manager.AddService(frontend);
            return frontend;
        }
    }
}
