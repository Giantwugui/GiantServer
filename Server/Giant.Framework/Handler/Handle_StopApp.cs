using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Giant.Framework
{
    [MessageHandler]
    public class Handle_StopApp : MHandler<Msg_GM_StopApp>
    {
        public override async Task Run(Session session, Msg_GM_StopApp message)
        {
            if (Scene.AppConfig.AppId == message.AppId)
            {
            }
            await Task.CompletedTask;
        }
    }
}
