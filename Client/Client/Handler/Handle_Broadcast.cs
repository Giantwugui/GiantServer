using Giant.Msg;
using Giant.Net;
using System;

namespace UdpSender.Handler
{
    [MessageHandler(ServerType.Client)]
    class Handle_Broadcast : MHandler<ZGC_Broadcast>
    {
        public override void Run(Session session, ZGC_Broadcast message)
        {
            Console.WriteLine(message.Message);
        }
    }
}
