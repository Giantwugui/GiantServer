using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_RegistService : MRpcHandler<Msg_RegistService_Req, Msg_RegistService_Rep>
    {
        public override void Run(Session session, Msg_RegistService_Req request, Msg_RegistService_Rep response)
        {
            Logger.Info($"regist service from appType {(AppType)request.AppType} appId {request.AppId}");

            response.Error = ErrorCode.Success;
            response.AppId = AppService.Instacne.AppId;
            response.AppType = (int)AppService.Instacne.AppType;

            AppService.Instacne.NetProxyManager.RegistBackendService((AppType)request.AppType, request.AppId, session);
        }
    }
}
