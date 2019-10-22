using Giant.Msg;
using Giant.Net;

namespace Server.Frame
{
    public abstract class BaseServer
    {
        public Session Session { get; set; }

        public void Write(IMessage message)
        {
            Session.Notify(message);
        }
    }
}
