using Giant.Msg;
using System;

namespace Giant.Net
{
    public interface IMHandler
	{
		Type GetMessageType();

		void Handle(Session session, IMessage message);
    }

    public abstract class MsgHandler<T> : IMHandler where T : class
    {
        public Type GetMessageType()
        {
            return typeof(T);
        }

        public abstract void Handle(Session session, IMessage message);
    }


}