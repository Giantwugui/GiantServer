using Giant.Msg;
using Giant.Net;
using Giant.Share;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_StopApp : MHandler<CA_StopApp>
    {
        public override void Run(Session session, CA_StopApp message)
        {
            if (AppService.Instacne.AppId == message.AppId)
            {
                AppService.Instacne.StopApp();
            }
        }
    }
}
