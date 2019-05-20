using Giant.Msg;
using System;

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
            Message msg = message as Message;

            this.Run(session, msg);
        }
    }

    //请求响应类消息
    public abstract class MRpcHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse
    {
        public abstract void Run(Session session, Request request, Action<Response> reply);

        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public void Handle(Session session, object message)
        {
            Request request = message as Request;

            this.Run(session, request, (Response response) =>
            {
                response.RpcId = request.RpcId;
                session.Reply(response);
            });
        }
    }


}