using Giant.Data;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Giant.Frame
{
    public class FrontendService
    {
        private long lastHeatBeatTime = TimeHelper.NowSeconds;

        public FrontendManager FrontendManager { get; private set; }

        private Session session;
        public Session Session => session;
        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public AppConfig AppConfig { get; private set; }

        public bool IsConnected => Session.IsConnected;

        public FrontendService(FrontendManager manager, AppType appType, int appId, AppConfig appConfig)
        {
            this.AppId = appId;
            this.AppType = appType;
            this.AppConfig = appConfig;
            this.FrontendManager = manager;
        }

        public void Start()
        {
            session = FrontendManager.NetProxyManager.Service.InnerNetworkService.Create(AppConfig.InnerAddress);
            session.OnConnectCallback += OnConnected;
            session.Start();
        }

        public void HeartBeat()
        {
            if (!IsConnected)
            {
                return;
            }

            if (TimeHelper.NowSeconds - lastHeatBeatTime < 10)
            {
                return;
            }

            HeartBeat_Ping ping = new HeartBeat_Ping
            {
                AppType = (int)this.AppType,
                AppId = this.AppId,
            };

            session.Notify(ping);
            lastHeatBeatTime = TimeHelper.NowSeconds;
        }

        private void OnConnected(Session session, bool connState)
        {
            if (connState)
            {
                RegistService();
            }
            else
            {
                ReConnect(3000);
            }
        }

        private async void ReConnect(int delayTime)
        {
            await Task.Delay(delayTime);
            Logger.Info($"app {AppType} {AppId} connect to {AppConfig.ApyType} {AppConfig.AppId} {session.RemoteIPEndPoint}");

            session.Start();
        }

        private async void RegistService()
        {
            Msg_RegistService_Req request = new Msg_RegistService_Req()
            {
                AppId = AppId,
                AppType = (int)AppType,
            };

            IResponse response = await Session.Call(request);
            Msg_RegistService_Rep message = response as Msg_RegistService_Rep;
            Logger.Info($"app {AppType} {AppId} registed to {(AppType)message.AppType} {message.AppId} success !");
        }
    }
}
