using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.Frame
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Ping : MHandler<Msg_HeartBeat_Ping>
    {
        public override async Task Run(Session session, Msg_HeartBeat_Ping message)
        {
            Logger.Info($"heart beat ping from appType {(AppType)message.AppType} appId {message.AppId}");

            Msg_HeartBeat_Pong pong = new Msg_HeartBeat_Pong()
            {
                AppType = (int)Framework.AppType,
                AppId = Framework.AppId,
            };

            session.Notify(pong);
            await Task.CompletedTask;
        }
    }

    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Pong : MHandler<Msg_HeartBeat_Pong>
    {
        public override async Task Run(Session session, Msg_HeartBeat_Pong message)
        {
            Logger.Info($"heart beat pong from appType {(AppType)message.AppType} appId {message.AppId}");
            await Task.CompletedTask;
        }
    }
}
