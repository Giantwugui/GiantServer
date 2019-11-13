using Giant.Data;
using Giant.Share;
using Server.Frame;

namespace Server.Gate
{
    public class ServerCreater : BaseServerCreater
    {
        public ServerCreater(BaseAppService service) : base(service)
        {
        }

        public override FrontendServer CreateFrontendServer(AppConfig config)
        {
            FrontendServerManager manager = AppService.NetProxyManager.GetFrontendServiceManager(config.AppType);
            switch (config.AppType)
            {
                case AppType.Account:
                    return new AccountServer(manager, config);
                case AppType.Map:
                    return new ZoneServer(manager, config);
                case AppType.Global:
                    return new GlobalServer(manager, config);
                case AppType.Manager:
                    return new ManagerServer(manager, config);
                default:
                    return base.CreateFrontendServer(config);
            }
        }
    }
}
