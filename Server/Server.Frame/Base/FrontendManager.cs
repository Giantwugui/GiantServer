using Giant.Share;

namespace Server.Frame
{
    public class FrontendManager
    {
        private readonly DepthMap<int, int, FrontendService> services = new DepthMap<int, int, FrontendService>();
        public NetProxyManager NetProxyManager { get; private set; }

        public FrontendManager(NetProxyManager netProxy)
        {
            this.NetProxyManager = netProxy;
        }

        public void Add(FrontendService frontend)
        {
            services.Add(NetProxyManager.AppId, NetProxyManager.SubId, frontend);
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
    }
}
