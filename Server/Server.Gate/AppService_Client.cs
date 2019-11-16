using Server.Frame;
using Giant.Net;
using Giant.Share;

namespace Server.Gate
{
    partial class AppService
    {
        public GlobalServer GlobalServer => NetProxyManager.GetBackendSinglePoint(AppType.Global, this.AppId) as GlobalServer;
        public AccountServer AccountServer => NetProxyManager.GetBackendSinglePoint(AppType.Account, this.AppId) as AccountServer;
        public ZoneServer ZoneServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, this.AppId) as ZoneServer;
        public ManagerServer ManagerServer => NetProxyManager.GetBackendSinglePoint(AppType.Map, this.AppId) as ManagerServer;

        public FrontendServerManager  ZoneServerManager  => NetProxyManager.GetFrontendServiceManager(AppType.Map);

        public override void OnAccept(Session session, bool isConnect)
        {
            if (isConnect)
            {
                ClientManager.Instance.Add2Watting(new Client(session));
            }
            else
            {
                Client client = ClientManager.Instance.GetClient(session.Id);
                client?.Offline();
            }
        }
    }
}
