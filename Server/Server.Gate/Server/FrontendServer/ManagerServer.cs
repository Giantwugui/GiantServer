using Giant.Data;
using Server.Frame;

namespace Server.Gate
{
    public class ManagerServer : FrontendServer
    {
        public ManagerServer(FrontendServerManager manager, AppConfig appConfig) : base(manager, appConfig)
        { }
    }
}
