using Giant.Net;
using Giant.Share;
using Server.Frame;

namespace Server.Gate
{
    partial class AppService
    {
        public GlobalServer GlobalServer => NetProxyManager.GetBackendSinglePoint(AppType.Global, AppId) as GlobalServer;
        public AccountServer AccountServer => NetProxyManager.GetBackendSinglePoint(AppType.Account, AppId) as AccountServer;
        public ZoneServer ZoneServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, AppId) as ZoneServer;
        public ManagerServer ManagerServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, AppId) as ManagerServer;

        public FrontendServerManager ZoneServerManager => NetProxyManager.GetFrontendServiceManager(AppType.Map);

        protected override void OnAccept(Session session, bool isConnect)
        {
            if (isConnect)
            {
            }
            else
            {
                Client client = ClientManager.Instance.GetClient(session.Id);
                client?.Offline();
            }
        }
    }
}
