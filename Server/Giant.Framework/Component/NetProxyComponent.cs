using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;
using Giant.Logger;
using Giant.Util;
using System.Collections.Generic;

namespace Giant.Framework
{
    public class NetProxyComponent : InitSystem
    {
        private readonly Dictionary<AppType, FrontendManagerComponent> frontendServices = new();
        private readonly Dictionary<AppType, BackendManagerComponent> backendServices = new();

        public static NetProxyComponent Instance { get; private set; }

        public FrontendComponent FrontendManagerServer => GetFrontendSinglePoint(AppType.Manager, Scene.AppConfig.AppId);


        public override void Init()
        {
            Instance = this;
            if (Scene.AppConfig.AppType == AppType.Global)
            {
                return;
            }

            AppConfig config = Scene.Pool.GetComponent<AppConfigDataList>().GetNetConfig(AppType.Global);
            if (config == null)
            {
                return;
            }

            FrontendComponent frontend = ComponentFactory.Create<FrontendComponent, AppConfig>(config);
            AddFrontend(frontend);
        }

        #region Frontend

        public FrontendManagerComponent GetFrontendServiceManager(AppType appType)
        {
            if (!frontendServices.TryGetValue(appType, out var manager))
            {
                manager = ComponentFactory.Create<FrontendManagerComponent>();
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

        public void Start()
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
                if (oldService.InstanceId == service.InstanceId)
                {
                    Log.Warn($"{service.AppType} {service.AppId} {service.SubId} regist to {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId} repeat !");
                    return;
                }
                else
                {
                    oldService.Dispose();
                    manager.RemoveService(service.AppId, service.SubId);
                }
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
                manager = ComponentFactory.Create<BackendManagerComponent>();
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
