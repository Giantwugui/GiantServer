using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler(AppType.Gate)]
    public class Handle_AG_ClientLogin : MHandler<Msg_AG_ClientLogin>
    {
        public override Task Run(Session session, Msg_AG_ClientLogin message)
        {
            ClientManager.Instance.WillEnter(new ClientEntry(message.Account, message.Token));
            return Task.CompletedTask;
        }
    }
}
