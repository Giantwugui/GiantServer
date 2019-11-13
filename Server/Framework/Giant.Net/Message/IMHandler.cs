using System;

namespace Giant.Net
{
    public interface IMHandler
    {
        Type GetMessageType();

        void Handle(Session session, object message);
    }
}