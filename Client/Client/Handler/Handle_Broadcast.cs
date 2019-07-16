using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;
using System.Threading.Tasks;

namespace Client
{
    [MessageHandler(AppType.AllServer)]
    class Handle_Broadcast : MHandler<ZGC_Broadcast>
    {
        public override async Task Run(Session session, ZGC_Broadcast message)
        {
            Console.WriteLine(message.Message);
        }
    }
}
