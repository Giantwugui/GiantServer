using Giant.Share;

namespace Server.Frame
{
    public class BackendManager
    {
        private readonly DepthMap<int, int, BackendService> backendServices = new DepthMap<int, int, BackendService>();

        public NetProxyManager NetProxyManager { get; private set; }

        public BackendManager(NetProxyManager netProxy)
        {
            this.NetProxyManager = netProxy;
        }

        public void RegistService(BackendService service)
        {
            backendServices.Add(service.AppId, service.SubId, service);
        }

        public void Remove(int appId, int subId)
        {
            backendServices.Remove(appId, subId);
        }
    }
}
