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
            AddFrontend(config);
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
            return manager.GetService(appId, subId);
        }

        public void AddFrontend(AppConfig config)
        {
            var manager = GetFrontendServiceManager(config.AppType);
            manager.AddService(config);
        }

        public void AddFrontend(FrontendService frontend)
        {
            var manager = GetFrontendServiceManager(frontend.AppConfig.AppType);
            manager.AddService(frontend);
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

        public void RegistBackendService(AppType appType, int appId, int subId, Session session)
        {
            if (!backendServices.TryGetValue(appType, out var manager))
            {
                manager = new BackendServiceManager(this);
                backendServices.Add(appType, manager);
            }
            else
            {
                if (manager.GetService(appId, subId) != null)
                {
                    Logger.Warn($"{appType} {appId} {subId} regist to {this.AppType} {this.AppId} {this.SubId} repeat !");
                    return;
                }
            }

            BackendService backend = new BackendService(manager, appType, appId, subId, session);
            manager.RegistService(backend);
            NotifyServices(backend);
        }

        public BackendService GetBackendWithCharge(AppType appType, int appId)
        {
            if (backendServices.TryGetValue(appType, out var manager))
            {

            }
            return null;
        }

        #endregion


        private void NotifyServices(BackendService backend)
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
