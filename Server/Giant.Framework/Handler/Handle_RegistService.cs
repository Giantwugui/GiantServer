using Giant.Core;
using Giant.EnumUtil;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System;
using System.Threading.Tasks;

namespace Giant.Framework
{
    [MessageHandler]
    public class Handle_RegistService : MHandler<Msg_RegistService_Req, Msg_RegistService_Rep>
    {
        public override async Task Run(Session session, Msg_RegistService_Req request, Msg_RegistService_Rep response, Action replay)
        {
            Log.Warn($"regist service from appType {(AppType)request.AppType} appId {request.AppId}");

            response.Error = ErrorCode.Success;
            response.AppId = Scene.AppConfig.AppId;
            response.SubId = Scene.AppConfig.SubId;
            response.AppType = (int)Scene.AppConfig.AppType;

            if (request == null)
            {
                Log.Error("request == null");
            }

            BackendComponent service = ComponentFactory.Create<BackendComponent, AppType, int, int, Session>(
                (AppType)request.AppType, request.AppId, request.SubId, session);
            NetProxyComponent.Instance.RegistBackendService(service);

            replay();

            await Task.CompletedTask;
        }
    }
}
