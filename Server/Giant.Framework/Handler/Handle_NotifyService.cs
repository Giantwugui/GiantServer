using Giant.Core;
using Giant.Model;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Giant.Framework
{
    [MessageHandler]
    public class Handle_NotifyService : MHandler<Msg_Service_Info>
    {
        public override async Task Run(Session session, Msg_Service_Info message)
        {
            AppType appType = (AppType)message.AppType;
            Log.Debug($"service notify from appType {appType} appId {message.AppId} subId {message.SubId}");

            if (Scene.Pool.GetComponent<NetGraphLibrary>().NeedConnect(Scene.AppConfig.AppType, Scene.AppConfig.AppId, appType, message.AppId))
            {
                AppConfig config = AppConfigLibrary.Instance.GetNetConfig(appType, message.AppId, message.SubId);
                if (config == null)
                {
                    Log.Error($"have not {appType} appId {message.AppId} subId {message.SubId} appConfig, create frontendService fail, please check !");
                    return;
                }

                FrontendComponent frontend = ComponentFactory.CreateComponent<FrontendComponent, AppConfig>(config);
                NetProxyComponent.Instance.GetFrontendServiceManager(appType).AddService(frontend);
                frontend.Start();
            }
            await Task.CompletedTask;
        }
    }
}
