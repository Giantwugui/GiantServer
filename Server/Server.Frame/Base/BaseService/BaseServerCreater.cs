using Giant.Data;
using Giant.Net;
using Giant.Share;

namespace Server.Frame
{
    public class BaseServerCreater
    {
        public NetProxyManager NetProxyManager => Framework.NetProxyManager;

        public BaseServerCreater()
        {
        }

        public virtual BackendServer CreateBackendServer(Session session, AppType appType, int appId, int subId)
        {
            BackendServerManager manager = NetProxyManager.GetBackendServiceManager(appType);
            BackendServer backend = new BackendServer(manager, appType, appId, subId, session);
            return backend;
        }

        public virtual FrontendServer CreateFrontendServer(AppConfig config)
        {
            var manager = NetProxyManager.GetFrontendServiceManager(config.AppType);
            FrontendServer frontend = new FrontendServer(manager, config);
            return frontend;
        }
    }
}
