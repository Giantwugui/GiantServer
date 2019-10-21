using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.Frame
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_RegistService : RpcMHandler<Msg_RegistService_Req, Msg_RegistService_Rep>
    {
        public override async Task Run(Session session, Msg_RegistService_Req request, Msg_RegistService_Rep response)
        {
            Logger.Warn($"regist service from appType {(AppType)request.AppType} appId {request.AppId}");

            response.Error = ErrorCode.Success;
            response.AppId = Framework.AppId;
            response.SubId = Framework.SubId;
            response.AppType = (int)Framework.AppType;

            if (request == null)
            {
                Logger.Error("request == null");
            }

            BackendService service = Framework.ServerCreater.CreateBackendServer(session, (AppType)request.AppType, request.AppId, request.SubId);
            Framework.BaseService.NetProxyManager.RegistBackendService(service);
            await Task.CompletedTask;
        }
    }
}
