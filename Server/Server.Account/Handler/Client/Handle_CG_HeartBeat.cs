using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.Account
{
    [MessageHandler]
    class Handle_CG_HeartBeat : MHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response)
        {
            response.Error = ErrorCode.Success;
            return Task.CompletedTask;
        }
    }
}
