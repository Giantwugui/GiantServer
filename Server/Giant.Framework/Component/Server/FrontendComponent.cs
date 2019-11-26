using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Framework
{
    public class FrontendComponent : BaseServerComponent, IInitSystem<FrontendManagerComponent, AppConfig>
    {
        private CancellationTokenSource cancellation;
        private long lastHeatBeatTime = TimeHelper.NowSeconds;

        public AppConfig AppConfig { get; private set; }
        public FrontendManagerComponent FrontendManager { get; private set; }
        public bool IsConnected => Session != null && Session.IsConnected;

        public void Init(FrontendManagerComponent manager, AppConfig appConfig)
        {
            AppConfig = appConfig;
            FrontendManager = manager;
        }

        public void Start()
        {
            InnerNetworkComponent component = Scene.Pool.GetComponent<InnerNetworkComponent>();
            Session = component.Create(AppConfig.InnerAddress);
            Session.OnConnectCallback += OnConnected;
            Session.Start();
        }

        public virtual void Update()
        {
            try
            {
                CheckHeartBeat();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void Send(IMessage message)
        {
            Session.Notify(message);
        }

        public Task<IResponse> Call(IRequest request)
        {
            return Session.Call(request);
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
                AppType = (int)Scene.AppConfig.AppType,
                AppId = Scene.AppConfig.AppId,
                SubId = Scene.AppConfig.SubId,
            };

            cancellation?.Cancel();
            cancellation = new CancellationTokenSource(3000);

            if (await Session.Call(ping, cancellation.Token) is Msg_HeartBeat_Pong message)
            {
                Log.Info($"heart beat pong from appType {(AppType)message.AppType} appId {message.AppId} subId {message.SubId}");
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
            Log.Warn($"app {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId} connect to {AppConfig.AppType} {AppConfig.AppId} {Session.RemoteIPEndPoint}");

            Start();
        }

        private async void RegistService()
        {
            Log.Warn($"app {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId} start registe to {AppConfig.AppType} {AppConfig.AppId} ...");

            Msg_RegistService_Req request = new Msg_RegistService_Req()
            {
                AppId = Scene.AppConfig.AppId,
                SubId = Scene.AppConfig.SubId,
                AppType = (int)Scene.AppConfig.AppType,
            };

            IResponse response = await Session.Call(request);
            Msg_RegistService_Rep message = response as Msg_RegistService_Rep;

            Log.Warn($"app {Scene.AppConfig.AppType} {Scene.AppConfig.AppId} {Scene.AppConfig.SubId} registed to {(AppType)message.AppType} {message.AppId} success !");
        }
    }
}
