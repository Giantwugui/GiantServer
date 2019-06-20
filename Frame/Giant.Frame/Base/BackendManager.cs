using Giant.Log;
using Giant.Net;
using Giant.Share;

namespace Giant.Frame
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
            Logger.Warn($"appType {NetProxyManager.AppType} {NetProxyManager.AppId} disconnect from {appInfo.AppType} {appInfo.AppId}");
        }

    }
}
