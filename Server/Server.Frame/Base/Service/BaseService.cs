using Giant.Msg;
using Giant.Net;

namespace Server.Frame
{
    public abstract class BaseService
    {
        public Session Session { get; set; }

        public void Write(IMessage message)
        {
            Session.Notify(message);
        }
    }
}
