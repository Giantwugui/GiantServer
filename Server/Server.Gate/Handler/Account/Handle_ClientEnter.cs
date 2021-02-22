using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    public class Handle_ClientEnter : MHandler<Msg_AG_ClientLogin>
    {
        public override Task Run(Session session, Msg_AG_ClientLogin message)
        {
            ClientManagerComponent.Instance.WillEnter(new ClientEnter(message.Account, message.Token));
            return Task.CompletedTask;
        }
    }
}
