using Giant.Core;
using Giant.Model;
using Giant.Msg;

namespace Giant.Framework
{
    public class BackendManagerComponent : BaseManagerComponent
    {
        private readonly DepthMap<int, int, BackendComponent> services = new DepthMap<int, int, BackendComponent>();

        public void RegistService(BackendComponent service)
        {
            services.Add(service.AppId, service.SubId, service);
        }

        public void RemoveService(int appId, int subId)
        {
            services.Remove(appId, subId);
        }

        public BackendComponent GetService(int appId, int subId)
        {
            services.TryGetValue(appId, subId, out var backend);
            return backend;
        }

        public override void NotifyServiceInfo(BackendComponent backend)
        {
            BackendComponent server;
            NetGraphLibComponent component = Scene.Pool.GetComponent<NetGraphLibComponent>();
            foreach (var app in services)
            {
                foreach (var service in app.Value)
                {
                    server = service.Value;
                    if (component.NeedConnect(server.AppType, server.AppId, backend.AppType, backend.AppId))
                    {
                        Msg_Service_Info msg = new Msg_Service_Info
                        {
                            AppType = (int)backend.AppType,
                            AppId = backend.AppId,
                            SubId = backend.SubId,
                        };
                        service.Value.Write(msg);
                    }

                    if (component.NeeAccept(server.AppType, server.AppId, backend.AppType, backend.AppId))
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
