using Giant.Core;
using Giant.Data;
using Giant.Log;
using Giant.Net;
using Giant.Share;
using System.Collections.Generic;

namespace Server.Frame
{
    public class NetProxyManager
    {
        private readonly Dictionary<AppType, FrontendServiceManager> frontendServices = new Dictionary<AppType, FrontendServiceManager>();
        private readonly Dictionary<AppType, BackendServiceManager> backendServices = new Dictionary<AppType, BackendServiceManager>();

        public BaseServerCreater ServerCreater { get; private set; }
        public BaseAppService Service { get; private set; }
        public AppType AppType => Service.AppType;
        public int AppId => Service.AppId;
        public int SubId => Service.SubId;

        public void Init(BaseAppService service)
        {
            this.Service = service;
            if (AppType == AppType.Global)
            {
                return;
            }
            var config = AppConfigLibrary.GetNetConfig(AppType.Global);
            if (config == null)
            {
                return;
            }

            FrontendService frontend = ServerCreater.CreateFrontendServer(config);
            AddFrontend(frontend);
        }

        public void Start()
        {
            StartFrontend();
        }

        public void SetServerCreater(BaseServerCreater creater)
        {
            this.ServerCreater = creater;
        }

        public void Update()
        {
            UpdateFrontend();
        }

        #region Frontend

        public FrontendServiceManager GetFrontendServiceManager(AppType appType)
        {
            if (!frontendServices.TryGetValue(appType, out var manager))
            {
                manager = new FrontendServiceManager(this);
                frontendServices.Add(appType, manager);
            }
            return manager;
        }

        public FrontendService GetFrontend(AppType appType, int appId, int subId)
        {
            var manager = GetFrontendServiceManager(appType);
            return manager?.GetService(appId, subId);
        }

        public FrontendService GetFrontendSinglePoint(AppType appType, int appId)
        {
            return GetFrontend(appType, appId, 0);
        }

        public void AddFrontend(FrontendService frontend)
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

        public void RegistBackendService(BackendService service)
        {
            BackendServiceManager manager = GetBackendServiceManager(service.AppType);
            BackendService oldService = manager.GetService(service.AppId, service.SubId);
            if (oldService != null)
            {
                Logger.Warn($"{service.AppType} {service.AppId} {service.SubId} regist to {this.AppType} {this.AppId} {this.SubId} repeat !");
                return;
            }

            manager.RegistService(service);
            NotifyServices(service);
        }

        public BackendService GetBackendWithCharge(AppType appType, int appId)
        {
            if (backendServices.TryGetValue(appType, out var manager))
            {
                return manager.GetService(appId, 0);
            }
            return null;
        }

        public BackendService GetBackend(AppType appType, int appId, int subId)
        {
            var manager = GetBackendServiceManager(appType);
            return manager?.GetService(appId, subId);
        }

        public FrontendService GetBackendSinglePoint(AppType appType, int appId)
        {
            return GetFrontend(appType, appId, 0);
        }

        public BackendServiceManager GetBackendServiceManager(AppType appType)
        {
            if (!backendServices.TryGetValue(appType, out var manager))
            {
                manager = new BackendServiceManager(this);
                backendServices.Add(appType, manager);
            }
            return manager;
        }

        #endregion


        public void NotifyServices(BackendService backend)
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
