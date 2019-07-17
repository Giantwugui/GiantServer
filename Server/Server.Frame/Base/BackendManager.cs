using Giant.Log;
using Giant.Net;
using Giant.Share;

namespace Server.Frame
{
    public class BackendManager
    {
        //各app内部连接
        private readonly DepthMap<AppType, long, AppInfo> backendSessions = new DepthMap<AppType, long, AppInfo>();

        public NetProxyManager NetProxyManager { get; private set; }

        public BackendManager(NetProxyManager netProxy)
        {
            this.NetProxyManager = netProxy;
        }

        public void RegistService(AppInfo appInfo)
        {
            appInfo.Session.OnConnectCallback += OnConnect;
            backendSessions.Add(appInfo.AppType, appInfo.SessionId, appInfo);
        }

        private void OnConnect(Session session, bool connectState)
        {
            if (connectState)
            {
                return;
            }

            if (!backendSessions.TryGetValue(session.Id, out AppInfo appInfo))
            {
                return;
            }

            backendSessions.Remove(appInfo.AppType, session.Id);

            Logger.Warn($"appType {appInfo.AppType} {appInfo.AppId} disconnect from {NetProxyManager.AppType} {NetProxyManager.AppId}");
        }

    }
}
