using Giant.Core;
using Giant.Logger;
using Giant.Net;

namespace Giant.Framework
{
    public class BackendComponent : BaseServerComponent, IInitSystem<BackendManagerComponent, AppType, int, int, Session>
    {
        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public int SubId { get; private set; }
        public BackendManagerComponent BackendManager { get; private set; }

        public void Init(BackendManagerComponent manager, AppType appType, int appId, int subId, Session session)
        {
            SubId = subId;
            AppId = appId;
            Session = session;
            AppType = appType;
            BackendManager = manager;
            Session.OnConnectCallback += OnDissConnect;
        }

        private void OnDissConnect(Session session, bool connectState)
        {
            if (connectState)
            {
                return;
            }

            BackendManager.RemoveService(AppId, SubId);

            Log.Warn($"appType {AppType} {AppId} disconnect from {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId}");
        }
    }
}
