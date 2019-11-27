using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Giant.Framework
{
    [MessageHandler]
    public class Handle_HeatBeat_Ping : RpcMHandler<Msg_HeartBeat_Ping, Msg_HeartBeat_Pong>
    {
        public override Task Run(Session session, Msg_HeartBeat_Ping request, Msg_HeartBeat_Pong response)
        {
            Log.Info($"heart beat ping from appType {(AppType)request.AppType} appId {request.AppId} subId {request.SubId}");

            response.AppType = (int)Scene.AppConfig.AppType;
            response.AppId = Scene.AppConfig.AppId;
            response.SubId = Scene.AppConfig.SubId;

            return Task.CompletedTask;
        }
    }
}
