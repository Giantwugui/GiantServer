using Giant.Msg;
using System;

namespace Giant.Net
{
    public interface IMHandler
	{
		Type GetMessageType();

		void Handle(Session session, IMessage message);
    }

    public abstract class MHandler<T> : IMHandler where T : class
    {
        public Type GetMessageType()
        {
            return typeof(T);
        }

        public abstract void Handle(Session session, IMessage message);
    }

    public abstract class MRpcHandler<Request, Response> : IMHandler where Request : IRequest where Response : IResponse
    {
        public abstract void Run(Session session, IRequest message, Action<Response> reply);

        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public void Handle(Session session, IMessage message)
        {
            IRequest request = message as IRequest;

            this.Run(session, request, (Response response) =>
            {
                response.RpcId = request.RpcId;
                session.Reply(response);
            });
        }
    }


}