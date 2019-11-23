using Giant.Core;
using Giant.Data;
using Giant.Log;
using Giant.Share;
using System.Collections.Generic;

namespace Server.Frame
{
    public class NetProxyManager
    {
        private readonly Dictionary<AppType, FrontendServerManager> frontendServices = new Dictionary<AppType, FrontendServerManager>();
        private readonly Dictionary<AppType, BackendServerManager> backendServices = new Dictionary<AppType, BackendServerManager>();

        public BaseAppService AppService { get; private set; }
        public AppType AppType => AppService.AppType;
        public int AppId => AppService.AppId;
        public int SubId => AppService.SubId;

        public NetProxyManager(BaseAppService service)
        {
            AppService = service;
        }

        public void Init()
        {
            if (AppType == AppType.Global)
            {
                return;
            }
            var config = AppConfigLibrary.GetNetConfig(AppType.Global);
            if (config == null)
            {
                return;
            }

            FrontendServer frontend = AppService.ServerFactory.CreateFrontendServer(config);
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

        public FrontendServerManager GetFrontendServiceManager(AppType appType)
        {
            if (!frontendServices.TryGetValue(appType, out var manager))
            {
                manager = new FrontendServerManager(this);
                frontendServices.Add(appType, manager);
            }
            return manager;
        }

        public FrontendServer GetFrontend(AppType appType, int appId, int subId)
        {
            var manager = GetFrontendServiceManager(appType);
            return manager?.GetService(appId, subId);
        }

        public FrontendServer GetFrontendSinglePoint(AppType appType, int appId)
        {
            return GetFrontend(appType, appId, 1);
        }

        public void AddFrontend(FrontendServer frontend)
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

        public void RegistBackendService(BackendServer service)
        {
            BackendServerManager manager = GetBackendServiceManager(service.AppType);
            BackendServer oldService = manager.GetService(service.AppId, service.SubId);
            if (oldService != null)
            {
                Logger.Warn($"{service.AppType} {service.AppId} {service.SubId} regist to {AppType} {AppId} {SubId} repeat !");
                return;
            }

            manager.RegistService(service);
            NotifyServices(service);
        }

        public BackendServer GetBackendWithCharge(AppType appType, int appId)
        {
            if (backendServices.TryGetValue(appType, out var manager))
            {
                return manager.GetService(appId, 0);
            }
            return null;
        }

        public BackendServer GetBackend(AppType appType, int appId, int subId)
        {
            var manager = GetBackendServiceManager(appType);
            return manager?.GetService(appId, subId);
        }

        public FrontendServer GetBackendSinglePoint(AppType appType, int appId)
        {
            return GetFrontend(appType, appId, 1);
        }

        public BackendServerManager GetBackendServiceManager(AppType appType)
        {
            if (!backendServices.TryGetValue(appType, out var manager))
            {
                manager = new BackendServerManager(this);
                backendServices.Add(appType, manager);
            }
            return manager;
        }

        #endregion


        public void NotifyServices(BackendServer backend)
        {
            //只允许global通知
            if (AppType != AppType.Global)
            {
                return;
            }

            frontendServices.ForEach(x => x.Value.NotifyServiceInfo(backend));
            backendServices.ForEach(x => x.Value.NotifyServiceInfo(backend));
        }
    }
}
