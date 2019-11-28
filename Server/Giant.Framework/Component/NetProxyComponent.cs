using Giant.Core;
using Giant.Logger;
using System.Collections.Generic;

namespace Giant.Framework
{
    [Event(EventType.InitDone)]
    public class NetProxyComponentEventSystem : Event
    {
        public override void Handle()
        {
            Scene.Pool.GetComponent<NetProxyComponent>().Handle();
        }
    }

    public class NetProxyComponent : Component, IInitSystem, IUpdateSystem
    {
        private readonly Dictionary<AppType, FrontendManagerComponent> frontendServices = new Dictionary<AppType, FrontendManagerComponent>();
        private readonly Dictionary<AppType, BackendManagerComponent> backendServices = new Dictionary<AppType, BackendManagerComponent>();

        public static NetProxyComponent Instance { get; private set; }

        public FrontendComponent FrontendManagerServer => GetFrontendSinglePoint(AppType.Manager, Scene.AppConfig.AppId);


        public void Init()
        {
            Instance = this;
            if (Scene.AppConfig.AppType == AppType.Global)
            {
                return;
            }
            AppConfig config = Scene.Pool.GetComponent<AppConfigComponent>().GetNetConfig(AppType.Global);
            if (config == null)
            {
                return;
            }

            FrontendComponent frontend = ComponentFactory.CreateComponent<FrontendComponent, AppConfig>(config);
            AddFrontend(frontend);
        }

        public void Handle()
        {
            StartFrontend();
        }

        public void Update(double t)
        {
        }

        #region Frontend

        public FrontendManagerComponent GetFrontendServiceManager(AppType appType)
        {
            if (!frontendServices.TryGetValue(appType, out var manager))
            {
                manager = ComponentFactory.CreateComponent<FrontendManagerComponent>();
                frontendServices.Add(appType, manager);
            }
            return manager;
        }

        public FrontendComponent GetFrontend(AppType appType, int appId, int subId)
        {
            var manager = GetFrontendServiceManager(appType);
            return manager?.GetService(appId, subId);
        }

        public FrontendComponent GetFrontendSinglePoint(AppType appType, int appId)
        {
            return GetFrontend(appType, appId, 1);
        }

        public void AddFrontend(FrontendComponent frontend)
        {
            var manager = GetFrontendServiceManager(frontend.AppConfig.AppType);
            manager?.AddService(frontend);
        }

        private void StartFrontend()
        {
            frontendServices.ForEach(x => x.Value.Start());
        }

        #endregion

        #region Backend

        public void RegistBackendService(BackendComponent service)
        {
            BackendManagerComponent manager = GetBackendServiceManager(service.AppType);
            BackendComponent oldService = manager.GetService(service.AppId, service.SubId);
            if (oldService != null)
            {
                Log.Warn($"{service.AppType} {service.AppId} {service.SubId} regist to {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId} repeat !");
                return;
            }

            manager.RegistService(service);
            NotifyServices(service);
        }

        public BackendComponent GetBackendWithCharge(AppType appType, int appId)
        {
            if (backendServices.TryGetValue(appType, out var manager))
            {
                return manager.GetService(appId, 0);
            }
            return null;
        }

        public BackendComponent GetBackend(AppType appType, int appId, int subId)
        {
            var manager = GetBackendServiceManager(appType);
            return manager?.GetService(appId, subId);
        }

        public FrontendComponent GetBackendSinglePoint(AppType appType, int appId)
        {
            return GetFrontend(appType, appId, 1);
        }

        public BackendManagerComponent GetBackendServiceManager(AppType appType)
        {
            if (!backendServices.TryGetValue(appType, out var manager))
            {
                manager = ComponentFactory.CreateComponent<BackendManagerComponent>();
                backendServices.Add(appType, manager);
            }
            return manager;
        }

        #endregion


        public void NotifyServices(BackendComponent backend)
        {
            //只允许global通知
            if (Scene.AppConfig.AppType != AppType.Global)
            {
                return;
            }

            frontendServices.ForEach(x => x.Value.NotifyServiceInfo(backend));
            backendServices.ForEach(x => x.Value.NotifyServiceInfo(backend));
        }
    }
}
