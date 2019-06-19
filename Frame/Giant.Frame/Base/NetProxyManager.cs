using Giant.Data;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Frame
{
    public class AppInfo
    {
        public AppType AppType{ get; set; }
        public int AppId{ get; set; }
        public Session Session { get; set; }
    }

    public class NetProxyManager
    {
        private long lastHeatBeatTime = TimeHelper.NowSeconds;

        //各app内部连接
        private readonly ListMap<AppType, FrontendService> frontendService = new ListMap<AppType, FrontendService>();
        private readonly DepthMap<AppType, int, AppInfo> backendSessions = new DepthMap<AppType, int, AppInfo>();

        public BaseService Service { get; private set; }

        public AppType AppType { get { return Service.AppType; } }
        public int AppId { get { return Service.AppId; } }


        public NetProxyManager(BaseService service)
        {
            this.Service = service;
        }

        public void Init()
        {
            var netPology = NetTopologyConfig.GetTopology(this.AppType);
            if (netPology == null)
            {
                return;
            }

            netPology.ForEach(pology =>
            {
                frontendService.Add(pology.ApyType, new FrontendService(this, this.AppType, this.AppId, pology));
            });
        }

        public void Start()
        {
            foreach (var kv in frontendService)
            {
                kv.Value.ForEach(sevice => sevice.Start());
            }
        }

        public void Update(float delayTime)
        {
            HeartBeat();
        }

        public void BackendRegist(AppType appType, int appId, Session session)
        { 
        }

        private void HeartBeat()
        {
            if (TimeHelper.NowSeconds - lastHeatBeatTime < 10)
            {
                return;
            }

            HeartBeat_Ping ping = new HeartBeat_Ping
            {
                AppType = (int)this.Service.AppType,
                AppId = this.Service.AppId,
            };

            foreach (var kv in frontendService)
            {
                if (kv.Value.Count == 0)
                {
                    continue;
                }

                foreach (var app in kv.Value)
                {
                    if (!app.Session.IsConnected)
                    {
                        continue;
                    }

                    app.Session.Send(ping);
                }
            }

            lastHeatBeatTime = TimeHelper.NowSeconds;
        }
    }
}
