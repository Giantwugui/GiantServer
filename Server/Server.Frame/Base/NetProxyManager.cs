using Giant.Data;
using Giant.Net;
using Giant.Share;
using System.Collections.Generic;

namespace Server.Frame
{
    public class NetProxyManager
    {
        private readonly Dictionary<AppType, FrontendManager> frontendServices = new Dictionary<AppType, FrontendManager>();
        private readonly Dictionary<AppType, BackendManager> backendServices = new Dictionary<AppType, BackendManager>();

        public BaseAppService Service { get; private set; }

        public AppType AppType => Framework.AppType;
        public int AppId => Framework.AppId;
        public int SubId => Framework.SubId;

        public void Start()
        {
            StartFrontend();
        }

        public void Update(float delayTime)
        {
            UpdateFrontend();
        }

        public void RegistBackendService(AppType appType, int appId, int subId, Session session)
        {
            if (!backendServices.TryGetValue(appType, out var manager))
            {
                manager = new BackendManager(this);
                backendServices.Add(AppType, manager);
            }
            manager.RegistService(new BackendService(manager) { AppType = appType, AppId = appId, SubId = subId, Session = session });
        }

        public void Init(BaseAppService service)
        {
            this.Service = service;

            if (AppType != AppType.Global)
            {
                return;
            }

            var netPology = NetTopologyConfig.GetTopology(this.AppType);
            if (netPology == null)
            {
                return;
            }

            AddFrontend(netPology);
        }

        private void StartFrontend()
        {
            frontendServices.ForEach(x => x.Value.Start());
        }

        private void UpdateFrontend()
        {
            frontendServices.ForEach(x => x.Value.Update());
        }

        private void AddFrontend(List<AppConfig> appConfigs)
        {
            appConfigs.ForEach(config =>
            {
                if (!frontendServices.TryGetValue(config.ApyType, out var frontendManager))
                {
                    frontendManager = new FrontendManager(this);
                    frontendServices.Add(config.ApyType, frontendManager);
                }

                frontendManager.Add(new FrontendService(frontendManager, config));
            });
        }
    }
}
