using Giant.Core;
using Giant.EnumUtil;
using Giant.Logger;
using Giant.Net;

namespace Giant.Framework
{
    public class BackendComponent : BaseServerComponent, IInitSystem<AppType, int, int, Session>
    {
        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public int SubId { get; private set; }

        public void Init(AppType appType, int appId, int subId, Session session)
        {
            SubId = subId;
            AppId = appId;
            Session = session;
            AppType = appType;
            session.OnConnectCallback += OnDisconnect;

            Scene.EventSystem.Handle(EventType.AffterBackend, this);
        }

        private void OnDisconnect(Session session, bool connectState)
        {
            if (connectState)
            {
                NetProxyComponent.Instance.GetBackendServiceManager(AppType).RemoveService(AppId, SubId);

                Log.Warn($"appType {AppType} {AppId} disconnect from {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId}");
            }
        }
    }
}
