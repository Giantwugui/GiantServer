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
            try
            {
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }

    [MessageHandler(AppType.AllServer)]
    public class Handle_HeatBeat_Pong : MHandler<HeartBeat_Pong>
    {
        public override void Run(Session session, HeartBeat_Pong message)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
