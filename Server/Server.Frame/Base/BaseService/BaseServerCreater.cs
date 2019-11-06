using Giant.Data;
using Giant.Net;
using Giant.Share;

namespace Server.Frame
{
    public class BaseServerCreater
    {
        public BaseAppService AppService { get; private set; }

        public BaseServerCreater(BaseAppService appService)
        {
            AppService = appService;
        }

        public virtual BackendServer CreateBackendServer(Session session, AppType appType, int appId, int subId)
        {
            BackendServerManager manager = AppService.NetProxyManager.GetBackendServiceManager(appType);
            BackendServer backend = new BackendServer(manager, appType, appId, subId, session);
            return backend;
        }

        public virtual FrontendServer CreateFrontendServer(AppConfig config)
        {
            var manager = AppService.NetProxyManager.GetFrontendServiceManager(config.AppType);
            FrontendServer frontend = new FrontendServer(manager, config);
            return frontend;
        }
    }
}
