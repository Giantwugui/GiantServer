using Giant.Log;
using Giant.Net;
using Giant.Share;

namespace Server.Frame
{
    public class BackendServer : BaseServer
    {
        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public int SubId { get; private set; }
        public BackendServerManager BackendManager { get; private set; }

        public BackendServer(BackendServerManager manager, AppType appType, int appId, int subId, Session session)
        {
            this.SubId = subId;
            this.AppId = appId;
            this.Session = session;
            this.AppType = appType;
            BackendManager = manager;
            Session.OnConnectCallback += OnConnect;
        }

        private void OnConnect(Session session, bool connectState)
        {
            if (connectState)
            {
                return;
            }

            BackendManager.RemoveService(AppId, SubId);

            Logger.Warn($"appType {AppType} {AppId} disconnect from {Framework.AppType} {Framework.AppId} {Framework.SubId}");
        }
    }
}
