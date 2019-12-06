using Giant.Logger;
using Giant.Msg;
using System;
using System.Threading.Tasks;

namespace Giant.Net
{
    //请求响应类消息
    public abstract class MHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse
    {
        public abstract Task Run(Session session, Request request, Response response);

        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public async void Handle(Session session, object message)
        {
            try
            {
                Request request = message as Request;
                Response response = Activator.CreateInstance<Response>();
                response.RpcId = request.RpcId;
                response.Error = ErrorCode.Success;

                try
                {
                    await Run(session, request, response);
                    session.Reply(response);
                }
                catch (Exception ex)
                {
                    response.Error = ErrorCode.RpcFail;
                    response.Message = ex.ToString();
                    session.Reply(response);

                    Log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }

}
