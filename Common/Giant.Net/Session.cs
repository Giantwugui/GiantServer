using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Giant.Log;
using Giant.Message;
using Giant.Share;

namespace Giant.Net
{
    public class Session : IDisposable
    {
        private int RpcId { get; set; }

        private readonly BaseChannel baseChannel;//通讯对象

        private readonly Dictionary<int, Action<IResponse>> responseCallback = new Dictionary<int, Action<IResponse>>();//消息回调

        public NetworkService NetworkService { get; private set; }

        public long Id { get; private set; }

        public Session(NetworkService networkService, BaseChannel baseChannel)
        {
            Id = baseChannel.Id;
            NetworkService = networkService;
            this.baseChannel = baseChannel;

            this.baseChannel.OnRead += OnRead;
            this.baseChannel.OnError += OnError;
        }


        public Task<IResponse> Call(IRequest message)
        {
            int rpcId = ++RpcId;
            message.RpcId = rpcId;

            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(message.GetType());

            TaskCompletionSource<IResponse> completionSource = new TaskCompletionSource<IResponse>();

            this.responseCallback[rpcId] = (response) =>
            {
                try
                {
                    if (response.Error == ErrorCode.ERR_Success)
                    {
                        completionSource.SetResult(response);
                    }
                    else
                    {
                        completionSource.SetException(new Exception($"ErrorCode {response.Error} Message {response.Message}"));
                    }
                }
                catch(Exception ex)
                {
                    completionSource.SetException(ex);
                }
            };

            this.Send(opcode, message);

            return completionSource.Task;
        }


        public void Send(IMessage message)
        {
            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(message.GetType());
            this.Send(opcode, message);
        }

        private void Send(ushort opcode, IMessage message)
        {
            byte[] msg = ProtoHelper.ToBytes(message);

            byte[] content = new byte[msg.Length + 2];
            content.WriteTo(0, opcode);
            content.WriteTo(2, msg);

            this.baseChannel.Send(content);
        }



        public void Dispose()
        {
            baseChannel.Dispose();

            //清空所有消息回调
            responseCallback.Clear();
        }

        private void OnRead(byte[] content)
        {
            //消息id
            ushort opcode = BitConverter.ToUInt16(content);

            Type msgType = NetworkService.MessageDispatcher.GetMessageType(opcode);

            IMessage message = ProtoHelper.FromBytes(content, msgType) as IMessage;

            if (message is IResponse response)
            {
                if (responseCallback.TryGetValue(opcode, out var action))
                {
                    action(response);
                    responseCallback.Remove(opcode);
                }
            }
            else
            {
                NetworkService.MessageDispatcher.Dispatch(this, opcode, message);
            }
        }

        private void OnError(object error)
        {
            switch (error)
            {
                case Exception ex:
                    {
                        Logger.Error(ex);
                    }
                    break;
                case int errorCode:
                    {
                        Logger.Error($"ErrorCode {errorCode}");
                    }
                    break;
                default:
                    Logger.Error(error);
                    break;
            }

            Logger.Error(error);
            NetworkService.Remove(this.Id);
        }

    }
}
