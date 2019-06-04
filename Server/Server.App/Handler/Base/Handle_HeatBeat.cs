using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Ping : MHandler<HeartBeat_Ping>
    {
        public override void Run(Session session, HeartBeat_Ping message)
        {
            Logger.Info($"heart beat ping from appType {message.AppType} appId {message.AppId} subId {message.SubId}");

            HeartBeat_Pong pong = new HeartBeat_Pong()
            {
                AppType = (int)AppService.Instacne.AppType,
                AppId = AppService.Instacne.AppId,
                SubId = AppService.Instacne.SubId,
            };

            session.Send(pong);
        }
    }

    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Pong : MHandler<HeartBeat_Pong>
    {
        public override void Run(Session session, HeartBeat_Pong message)
        {
            try
            {
                Logger.Info($"heart beat pong from appType {message.AppType} appId {message.AppId} subId {message.SubId}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
