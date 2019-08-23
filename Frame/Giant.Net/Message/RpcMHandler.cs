using Giant.Log;
using Giant.Msg;
using System;
using System.Threading.Tasks;

namespace Giant.Net
{
    //请求响应类消息
    public abstract class RpcMHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse
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

                try
                {
                    await this.Run(session, request, response);
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
