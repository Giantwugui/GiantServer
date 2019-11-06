using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;
using Giant.Data;

namespace Server.Frame
{
    [MessageHandler]
    public class Handle_NotifyService : MHandler<Msg_Service_Info>
    {
        public override async Task Run(Session session, Msg_Service_Info message)
        {
            AppType appType = (AppType)message.AppType;
            Logger.Debug($"service notify from appType {appType} appId {message.AppId} subId {message.SubId}");

            if (NetTopologyLibrary.NeedConnect(Framework.AppType, Framework.AppId, appType, message.AppId))
            {
                AppConfig config = AppConfigLibrary.GetNetConfig(appType, message.AppId, message.SubId);
                if (config == null)
                {
                    Logger.Error($"have not {appType} appId {message.AppId} subId {message.SubId} appConfig, create frontendService fail, please check !");
                    return;
                }

                FrontendServer frontend = Framework.ServerCreater.CreateFrontendServer(config);
                Framework.NetProxyManager.GetFrontendServiceManager(appType).AddService(frontend);
                frontend.Start();
            }
            await Task.CompletedTask;
        }
    }
}
