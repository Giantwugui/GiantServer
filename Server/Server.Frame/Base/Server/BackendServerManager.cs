using Giant.Core;
using Giant.Data;
using Giant.Msg;

namespace Server.Frame
{
    public class BackendServerManager : BaseServerManager
    {
        private readonly DepthMap<int, int, BackendServer> services = new DepthMap<int, int, BackendServer>();

        public BackendServerManager(NetProxyManager manager) : base(manager)
        {
        }

        public void RegistService(BackendServer service)
        {
            services.Add(service.AppId, service.SubId, service);
        }

        public void RemoveService(int appId, int subId)
        {
            services.Remove(appId, subId);
        }

        public BackendServer GetService(int appId, int subId)
        {
            services.TryGetValue(appId, subId, out var backend);
            return backend;
        }

        public override void NotifyServiceInfo(BackendServer backend)
        {
            BackendServer server;
            foreach (var app in services)
            {
                foreach (var service in app.Value)
                {
                    server = service.Value;
                    if (NetTopologyLibrary.NeedConnect(server.AppType, server.AppId, backend.AppType, backend.AppId))
                    {
                        Msg_Service_Info msg = new Msg_Service_Info
                        {
                            AppType = (int)backend.AppType,
                            AppId = backend.AppId,
                            SubId = backend.SubId,
                        };
                        service.Value.Write(msg);
                    }

                    if (NetTopologyLibrary.NeeAccept(server.AppType, server.AppId, backend.AppType, backend.AppId))
                    {
                        Msg_Service_Info msg = new Msg_Service_Info
                        {
                            AppType = (int)server.AppType,
                            AppId = server.AppId,
                            SubId = server.SubId,
                        };
                        backend.Write(msg);
                    }
                }
            }
        }
    }
}
