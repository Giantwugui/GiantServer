using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Client
{
    [MessageHandler]
    class Handle_Broadcast : MHandler<ZGC_Broadcast>
    {
        public override async Task Run(Session session, ZGC_Broadcast message)
        {
            Player player = PlayerManagerComponent.Instance.GetPlayer(session);

            await Task.CompletedTask;
        }
    }
}
