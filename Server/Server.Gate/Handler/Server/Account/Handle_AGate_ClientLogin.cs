using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    public class Handle_AGate_ClientLogin : MHandler<Msg_AG_ClientLogin>
    {
        public override Task Run(Session session, Msg_AG_ClientLogin message)
        {
            ClientManager.Instance.WillEnter(new ClientEnter(message.Account, message.Token));
            return Task.CompletedTask;
        }
    }
}
