using Giant.Log;
using System;
using System.Threading.Tasks;

namespace Giant.Net
{
    //通知类消息
    public abstract class MHandler<Message> : IMHandler where Message : class
    {
        public abstract Task Run(Session session, Message message);

        public Type GetMessageType()
        {
            return typeof(Message);
        }

        public async void Handle(Session session, object message)
        {
            try
            {
                Message msg = message as Message;
                await this.Run(session, msg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}