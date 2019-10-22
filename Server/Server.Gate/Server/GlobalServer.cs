using Giant.Data;
using Server.Frame;

namespace Server.Gate
{
    public class GlobalServer : FrontendServer
    {
        public GlobalServer(FrontendServerManager manager, AppConfig appConfig) : base(manager, appConfig)
        { }
    }
}
