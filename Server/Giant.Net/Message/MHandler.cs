using Giant.Logger;
using Giant.Msg;
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
                await Run(session, msg);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }

    //请求响应类消息
    public abstract class MHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse
    {
        public abstract Task Run(Session session, Request request, Response response, Action replay);

        public Type GetMessageType()
        {
            return typeof(Request);
        }

        public async void Handle(Session session, object message)
        {
            try
            {
                long instanceId = session.InstanceId;
                Request request = message as Request;
                Response response = Activator.CreateInstance<Response>();

                void Reply()
                {
                    // 等回调回来,session可以已经断开了,所以需要判断session InstanceId是否一样
                    if (session.InstanceId != instanceId)
                    {
                        return;
                    }

                    response.RpcId = request.RpcId;
                    response.Error = ErrorCode.Success;

                    session.Reply(response);
                }

                try
                {
                    await Run(session, request, response, Reply);
                }
                catch (Exception ex)
                {
                    response.Error = ErrorCode.RpcFail;
                    response.Message = ex.ToString();
                    Reply();

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