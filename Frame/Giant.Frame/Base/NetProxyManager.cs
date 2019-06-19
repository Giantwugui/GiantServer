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
        private long lastCheck = TimeHelper.NowSeconds;
        private long lastHeatBeatTime = TimeHelper.NowSeconds;

        //需要注册的session
        private readonly List<AppInfo> waitRegistSessions = new List<AppInfo>();

        //各app内部连接
        private readonly ListMap<AppType, AppInfo> frontSessions = new ListMap<AppType, AppInfo>();
        private readonly DepthMap<AppType, int, AppInfo> backendSessions = new DepthMap<AppType, int, AppInfo>();

        public BaseService Service { get; private set; }

        public AppType AppType { get { return Service.AppType; } }


        public NetProxyManager(BaseService service)
        {
            this.Service = service;
        }

        public void Init()
        {
            Session session;
            var netPology = NetTopologyConfig.GetTopology(this.AppType);
            if (netPology == null)
            {
                return;
            }

            netPology.ForEach(pology =>
            {
                session = this.Service.InnerNetworkService.GetSession(pology.InnerAddress);
                Add2Regist(new AppInfo() { AppType = pology.AppyType, AppId = pology.AppId, Session = session });
            });
        }

        public void Update(float delayTime)
        {
            HeartBeat();
            RegistSession();
        }

        public void AppRegist(AppType appType, int appId, Session session)
        { 
        }

        public void SessionDisconnect(Session session)
        {
        }

        private void Add2Regist(AppInfo regist)
        {
            waitRegistSessions.Add(regist);
        }

        private async void RegistSession()
        {
            if (TimeHelper.NowSeconds - lastCheck < 2)
            {
                return;
            }

            foreach (var kv in frontSessions)
            {
                if (kv.Value.Count == 0)
                {
                    continue;
                }

                //断开连接了的
                waitRegistSessions.AddRange(kv.Value.Where(app => !app.Session.IsConnected));
            }

            if (waitRegistSessions.Count <= 0) return;
            waitRegistSessions.ForEach(app => backendSessions.Remove(app.AppType, app.AppId));

            List<AppInfo> removeList = new List<AppInfo>();
            foreach (var registInfo in waitRegistSessions)
            {
                if (registInfo.Session.IsConnected)
                {
                    Msg_RegistService_Req reqyest = new Msg_RegistService_Req()
                    {
                        AppId = Service.AppId,
                        AppType = (int)Service.AppType,
                    };

                    IResponse response = await registInfo.Session.Call(reqyest);
                    Msg_RegistService_Rep message = response as Msg_RegistService_Rep;

                    AppType regist2AppType = (AppType)message.AppType;

                    RegistSuccess(registInfo);
                    removeList.Add(registInfo);
                    Logger.Info($"app {Service.AppType} {Service.AppId} regist to {regist2AppType} {message.AppId} success !");
                }
                else
                {
                    registInfo.Session.Start();
                    Logger.Info($"app {Service.AppType} {Service.AppId} connect to {registInfo.AppType} {registInfo.AppId} !");
                }
            }

            removeList.ForEach(session => waitRegistSessions.Remove(session));
            lastCheck = TimeHelper.NowSeconds;
        }

        private void RegistSuccess(AppInfo appInfo)
        {
            frontSessions.Add(appInfo.AppType, appInfo);
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

            foreach (var kv in frontSessions)
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
