using Giant.Log;
using Giant.Msg;
using System;

namespace Giant.Net
{
    //请求响应类消息
    public abstract class MRpcHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse
    {
        public abstract void Run(Session session, Request request, Response response);

        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public void Handle(Session session, object message)
        {
            try
            {
                Request request = message as Request;
                Response response = Activator.CreateInstance(typeof(Response)) as Response;
                response.RpcId = request.RpcId; 

                try
                {
                    this.Run(session, request, response);
                    session.Reply(response);
                }
                catch (Exception ex)
                {
                    response.Error = ErrorCode.RpcFail;
                    response.Message = ex.ToString();
                    session.Reply(response);

                    Logger.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }

}
