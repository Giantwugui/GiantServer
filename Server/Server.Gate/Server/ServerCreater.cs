using Giant.Data;
using Giant.Share;
using Server.Frame;

namespace Server.Gate
{
    public class ServerCreater : BaseServerCreater
    {
        public ServerCreater() : base()
        { 
        }

        public override FrontendServer CreateFrontendServer(AppConfig config)
        {
            FrontendServerManager manager = NetProxyManager.GetFrontendServiceManager(config.AppType);
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
