using Giant.Data;
using Giant.Net;
using Giant.Share;

namespace Giant.Frame
{
    public class AppInfo
    {
        public AppType AppType { get; set; }
        public int AppId { get; set; }
        public Session Session { get; set; }
        public long SessionId => Session.Id;
    }

    public class NetProxyManager
    {
        public BaseService Service { get; private set; }
        public FrontendManager FrontendManager { get; private set; }
        public BackendManager BackendManager { get; private set; }

        public AppType AppType { get { return Service.AppType; } }
        public int AppId { get { return Service.AppId; } }


        public NetProxyManager(BaseService service)
        {
            this.Service = service;
            this.FrontendManager = new FrontendManager(this);
            this.BackendManager = new BackendManager(this);
        }

        public void Init()
        {
            var netPology = NetTopologyConfig.GetTopology(this.AppType);
            if (netPology == null)
            {
                return;
            }

            netPology.ForEach(topology => this.FrontendManager.Add(topology));
        }

        public void Start()
        {
            FrontendManager.Start();
        }

        public void Update(float delayTime)
        {
            this.FrontendManager.Update(delayTime);
        }

        public void RegistBackendService(AppType appType, int appId, Session session)
        {
            this.BackendManager.RegistService(new AppInfo() { AppType = appType, AppId = appId, Session = session });
        }
    }
}
