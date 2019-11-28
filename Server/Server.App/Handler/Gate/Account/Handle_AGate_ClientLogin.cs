using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler]
    public class Handle_AGate_ClientLogin : MHandler<Msg_AG_ClientLogin>
    {
        public override Task Run(Session session, Msg_AG_ClientLogin message)
        {
            ClientManagerComponent.Instance.WillEnter(new ClientEnter(message.Account, message.Token));
            return Task.CompletedTask;
        }
    }
}
