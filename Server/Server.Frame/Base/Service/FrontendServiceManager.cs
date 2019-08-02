using Giant.Share;
using Giant.Data;
using Giant.Msg;

namespace Server.Frame
{
    public class FrontendServiceManager : BaseServiceManager
    {
        private readonly DepthMap<int, int, FrontendService> services = new DepthMap<int, int, FrontendService>();

        public FrontendServiceManager(NetProxyManager manager) : base(manager)
        {
        }

        public void AddService(AppConfig config)
        {
            AddService(new FrontendService(this, config));
        }

        public void AddService(FrontendService frontend)
        {
            services.Add(NetProxyManager.AppId, NetProxyManager.SubId, frontend);
        }

        public FrontendService GetService(int appId, int subId)
        {
            services.TryGetValue(appId, subId, out var backend);
            return backend;
        }

        public void Start()
        {
            foreach (var kv in services)
            {
                if (kv.Value.Count == 0)
                {
                    continue;
                }

                kv.Value.ForEach(x => x.Value.Start());
            }
        }

        public void Update()
        {
            foreach (var kv in services)
            {
                if (kv.Value.Count == 0)
                {
                    continue;
                }

                kv.Value.ForEach(x => x.Value.Update());
            }
        }

        public override void NotifyServiceInfo(BackendService backend)
        {
            AppConfig config;
            foreach (var app in services)
            {
                foreach (var service in app.Value)
                {
                    config = service.Value.AppConfig;
                    if (NetTopologyLibrary.NeeConnect(config.AppType, config.AppId, backend.AppType, backend.AppId))
                    {
                        Msg_Service_Info msg = new Msg_Service_Info
                        {
                            AppType = (int)backend.AppType,
                            AppId = backend.AppId,
                            SubId = backend.SubId,
                        };
                        service.Value.Write(msg);
                    }

                    if (NetTopologyLibrary.NeeAccept(config.AppType, config.AppId, backend.AppType, backend.AppId))
                    {
                        Msg_Service_Info msg = new Msg_Service_Info
                        {
                            AppType = (int)config.AppType,
                            AppId = config.AppId,
                            SubId = config.SubId,
                        };
                        backend.Write(msg);
                    }
                }
            }
        }
    }
}
