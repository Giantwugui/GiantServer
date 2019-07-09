using Giant.Msg;
using System;
using Giant.Log;
using System.Threading.Tasks;

namespace Giant.Net
{
    public interface IMHandler
	{
		Type GetMessageType();

		void Handle(Session session, object message);
    }

    //通知类消息
    public abstract class MHandler<Message> : IMHandler where Message : class
    {
        public abstract void Run(Session session, Message message);

        public Type GetMessageType()
        {
            return typeof(Message);
        }

        public void Handle(Session session, object message)
        {
            try
            {
                Message msg = message as Message;
                this.Run(session, msg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}