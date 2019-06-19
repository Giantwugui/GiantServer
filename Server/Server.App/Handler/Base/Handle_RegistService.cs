using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_RegistService : MRpcHandler<Msg_RegistService_Req, Msg_RegistService_Rep>
    {
        public override void Run(Session session, Msg_RegistService_Req request, Action<Msg_RegistService_Rep> reply)
        {
            try
            {
                Logger.Info($"regist service from appType {(AppType)request.AppType} appId {request.AppId}");
                Msg_RegistService_Rep response = new Msg_RegistService_Rep()
                {
                    RpcId = request.RpcId,
                    Error = (int)ErrorCode.Success,
                    AppId = AppService.Instacne.AppId,
                    AppType = (int)AppService.Instacne.AppType,
                };

                reply(response);

                AppService.Instacne.NetProxyManager.AppRegist((AppType)request.AppType, request.AppId, session);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
