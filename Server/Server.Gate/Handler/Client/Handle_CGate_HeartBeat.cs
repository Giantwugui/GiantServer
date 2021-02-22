using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    class Handle_CGate_HeartBeat : MHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response, Action replay)
        {
            response.Error = ErrorCode.Success;
            replay();

            return Task.CompletedTask;
        }
    }
}
