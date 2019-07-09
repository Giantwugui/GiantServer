using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_StopApp : MHandler<CA_StopApp>
    {
        public override async Task Run(Session session, CA_StopApp message)
        {
            if (AppService.Instacne.AppId == message.AppId)
            {
                AppService.Instacne.StopApp();
            }
        }
    }
}
