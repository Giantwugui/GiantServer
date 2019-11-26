using Giant.Core;
using Giant.Logger;
using System.Collections.Generic;

namespace Giant.Framework
{
    public class NetProxyComponent : Component, IInitSystem
    {
        private readonly Dictionary<AppType, FrontendManagerComponent> frontendServices = new Dictionary<AppType, FrontendManagerComponent>();
        private readonly Dictionary<AppType, BackendManagerComponent> backendServices = new Dictionary<AppType, BackendManagerComponent>();

        public void Init()
        {
            if (Scene.AppConfig.AppType == AppType.Global)
            {
                return;
            }
            AppConfig config = Scene.Pool.GetComponent<AppConfigComponent>().GetNetConfig(AppType.Global);
            if (config == null)
            {
                return;
            }

            FrontendManagerComponent component = GetFrontendServiceManager(config.AppType);
            FrontendComponent frontend = ComponentFactory.CreateComponent<FrontendComponent, FrontendManagerComponent, AppConfig>(component, config);
            AddFrontend(frontend);
        }

        public void Start()
        {
            StartFrontend();
        }

        public void Update()
        {
            UpdateFrontend();
        }

        #region Frontend

        public FrontendManagerComponent GetFrontendServiceManager(AppType appType)
        {
            if (!frontendServices.TryGetValue(appType, out var manager))
            {
                manager = ComponentFactory.CreateComponent<FrontendManagerComponent, NetProxyComponent>(this);
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

        private void UpdateFrontend()
        {
            frontendServices.ForEach(x => x.Value.Update());
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
                manager = ComponentFactory.CreateComponent<BackendManagerComponent, NetProxyComponent>(this);
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
