using Giant.Core;
using Giant.Msg;
using Giant.Net;

namespace Giant.Framework
{
    public abstract class BaseServerComponent : Component
    {
        public Session Session { get; set; }

        public void Write(IMessage message)
        {
            Session.Notify(message);
        }
    }
}
