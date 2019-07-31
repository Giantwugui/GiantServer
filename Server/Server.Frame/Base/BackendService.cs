using Giant.Log;
using Giant.Net;
using Giant.Share;

namespace Server.Frame
{
    public class BackendService
    {
        public AppType AppType { get; set; }
        public int AppId { get; set; }
        public int SubId { get; set; }
        public Session Session { get; set; }
        public BackendManager BackendManager { get; private set; }

        public BackendService(BackendManager manager)
        {
            BackendManager = manager;
            Session.OnConnectCallback += OnConnect;
        }

        private void OnConnect(Session session, bool connectState)
        {
            if (connectState)
            {
                return;
            }

            BackendManager.Remove(AppId, SubId);

            Logger.Warn($"appType {AppType} {AppId} disconnect from {Framework.AppType} {Framework.AppId} {Framework.SubId}");
        }
    }
}
