using Giant.Data;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Frame
{
    public class FrontendService
    {
        private CancellationTokenSource cancellation;
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

        public void Update()
        {
            try
            {
                CheckHeartBeat();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void CheckHeartBeat()
        {
            if (!IsConnected)
            {
                return;
            }

            if (TimeHelper.NowSeconds - lastHeatBeatTime < 30)
            {
                return;
            }

            HeartBeat();
            lastHeatBeatTime = TimeHelper.NowSeconds;
        }

        private async void HeartBeat()
        {
            Msg_HeartBeat_Ping ping = new Msg_HeartBeat_Ping
            {
                AppType = (int)this.AppType,
                AppId = this.AppId,
            };

            cancellation?.Cancel();
            cancellation = new CancellationTokenSource(3000);

            if (await session.Call(ping, cancellation.Token) is Msg_HeartBeat_Pong message)
            {
                Logger.Info($"heart beat pong from appType {(AppType)message.AppType} appId {message.AppId}");
            }

            cancellation.Dispose();
            cancellation = null;
        }

        private void OnConnected(Session session, bool connState)
        {
            if (connState)
            {
                RegistService();
            }
            else
            {
                CheckConnect();
            }
        }

        private async void CheckConnect()
        {
            await Task.Delay(3000);//3后重新连接
            Logger.Warn($"app {AppType} {AppId} connect to {AppConfig.ApyType} {AppConfig.AppId} {session.RemoteIPEndPoint}");

            this.Start();
        }

        private async void RegistService()
        {
            Logger.Warn($"app {AppType} {AppId} start registe to {AppConfig.ApyType} {AppConfig.AppId} ...");

            Msg_RegistService_Req request = new Msg_RegistService_Req()
            {
                AppId = AppId,
                AppType = (int)AppType,
            };

            IResponse response = await Session.Call(request);
            Msg_RegistService_Rep message = response as Msg_RegistService_Rep;

            Logger.Warn($"app {AppType} {AppId} registed to {(AppType)message.AppType} {message.AppId} success !");
        }
    }
}
