using Giant.Data;
using Giant.Msg;
using Giant.Share;

namespace Server.Frame
{
    public class BackendServiceManager : BaseServiceManager
    {
        private readonly DepthMap<int, int, BackendService> services = new DepthMap<int, int, BackendService>();

        public NetProxyManager NetProxyManager { get; private set; }

        public BackendServiceManager(NetProxyManager netProxy)
        {
            this.NetProxyManager = netProxy;
        }

        public void RegistService(BackendService service)
        {
            services.Add(service.AppId, service.SubId, service);
        }

        public void Remove(int appId, int subId)
        {
            services.Remove(appId, subId);
        }

        public BackendService GetService(int appId, int subId)
        {
            services.TryGetValue(appId, subId, out var backend);
            return backend;
        }

        public override void NotifyServiceInfo(BackendService backend)
        {
            Msg_Service_Info msg = new Msg_Service_Info
            {
                AppType = (int)backend.AppType,
                AppId = backend.AppId,
                SubId = backend.SubId,
            };

            BackendService config;
            foreach (var app in services)
            {
                foreach (var service in app.Value)
                {
                    config = service.Value;
                    if (NetTopologyLibrary.NeeConnect(config.AppType, config.AppId, backend.AppType, backend.AppId))
                    {
                        service.Value.Write(msg);
                    }

                    if (NetTopologyLibrary.NeeAccept(config.AppType, config.AppId, backend.AppType, backend.AppId))
                    {
                        backend.Write(msg);
                    }
                }
            }
        }
    }
}
