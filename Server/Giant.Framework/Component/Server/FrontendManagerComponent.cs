using Giant.Core;
using Giant.Model;
using Giant.Msg;

namespace Giant.Framework
{
    public class FrontendManagerComponent : BaseManagerComponent
    {
        private readonly DepthMap<int, int, FrontendComponent> services = new DepthMap<int, int, FrontendComponent>();

        public void AddService(FrontendComponent frontend)
        {
            if (services.TryGetValue(frontend.AppConfig.AppId, frontend.AppConfig.SubId, out var oldComponent))
            {
                oldComponent.Dispose();
                services.Remove(frontend.AppConfig.AppId, frontend.AppConfig.SubId);
            }

            services.Add(frontend.AppConfig.AppId, frontend.AppConfig.SubId, frontend);
        }

        public FrontendComponent GetService(int appId, int subId)
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

        public override void NotifyServiceInfo(BackendComponent backend)
        {
            AppConfig config;
            NetGraphLibComponent component = Scene.Pool.GetComponent<NetGraphLibComponent>();
            foreach (var app in services)
            {
                foreach (var service in app.Value)
                {
                    config = service.Value.AppConfig;
                    if (component.NeedConnect(config.AppType, config.AppId, backend.AppType, backend.AppId))
                    {
                        Msg_Service_Info msg = new Msg_Service_Info
                        {
                            AppType = (int)backend.AppType,
                            AppId = backend.AppId,
                            SubId = backend.SubId,
                        };
                        service.Value.Write(msg);
                    }

                    if (component.NeeAccept(config.AppType, config.AppId, backend.AppType, backend.AppId))
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
