using Giant.Msg;
using Giant.Net;

namespace UdpSender.Handler
{
    [MessageHandler(ServerType.Client)]
    class Handle_Broadcast : MsgHandler<ZGC_Broadcast>
    {
        public override void Handle(Session session, IMessage message)
        {
            RC_LOGIN login = message as RC_LOGIN;

            if (login.Error == ErrorCode.ERR_Success)
            {
                //Console.WriteLine($" account {login.Account} pass {login.Password}");
            }
        }
    }
}
