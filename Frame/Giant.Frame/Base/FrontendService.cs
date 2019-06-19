using Giant.Data;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Giant.Frame
{
    public class FrontendService
    {
        private NetProxyManager netProxy;
        private Session session;
        public Session Session => session;
        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public AppConfig AppConfig { get; private set; }

        public bool IsConnected => Session.IsConnected;

        public FrontendService(NetProxyManager netProxy, AppType appType, int appId, AppConfig appConfig)
        {
            this.netProxy = netProxy;
            this.AppType = appType;
            this.AppId = appId;
            this.AppConfig = appConfig;
        }

        public void Start()
        {
            session = this.netProxy.Service.InnerNetworkService.GetSession(AppConfig.InnerAddress);
            session.OnConnectCallback += OnConnected;
            session.Start();
        }

        private void OnConnected(Session session, bool connState)
        {
            if (connState)
            {
                RegistService();
            }
            else
            {
                ReConnect(2);
            }
        }

        private async void ReConnect(int delayTime)
        {
            Logger.Info($"app {AppType} {AppId} connect to {AppConfig.ApyType} {AppConfig.AppId} {session.RemoteIPEndPoint}");
            await Task.Delay(delayTime);
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
            Logger.Info($"app {AppType} {AppId} regist to {(AppType)message.AppType} {message.AppId} success !");
        }
    }
}
