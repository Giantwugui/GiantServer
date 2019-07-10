using Giant.Data;
using Giant.Share;

namespace Giant.Frame
{
    public class FrontendManager
    {
        private readonly ListMap<AppType, FrontendService> services = new ListMap<AppType, FrontendService>();
        public NetProxyManager NetProxyManager { get; private set; }

        public FrontendManager(NetProxyManager netProxy)
        {
            this.NetProxyManager = netProxy;
        }

        public void Add(AppConfig appConfig)
        {
            services.Add(NetProxyManager.AppType, new FrontendService(this, NetProxyManager.AppType, NetProxyManager.AppId, appConfig));
        }

        public void Start()
        {
            foreach (var kv in services)
            {
                kv.Value.ForEach(sevice => sevice.Start());
            }
        }

        public void Update(float delayTime)
        {
            //心跳
            foreach (var kv in services)
            {
                kv.Value.ForEach(service => service.Update());
            }
        }
    }
}
