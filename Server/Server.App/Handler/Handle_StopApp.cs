using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_StopApp : MHandler<CA_StopApp>
    {
        public override void Run(Session session, CA_StopApp message)
        {
            try
            {
                if (Service.Instacne.AppId == message.MainId && Service.Instacne.SubId == message.SubId)
                {
                    Service.Instacne.StopApp();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
