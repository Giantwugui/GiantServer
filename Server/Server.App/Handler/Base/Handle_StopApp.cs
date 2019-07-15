using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler(AppType.AllServer)]
    public class Handle_StopApp : MHandler<Msg_GM_StopApp>
    {
        public override async Task Run(Session session, Msg_GM_StopApp message)
        {
            if (AppService.Instacne.AppId == message.AppId)
            {
                AppService.Instacne.StopApp();
            }
            await Task.CompletedTask;
        }
    }
}
