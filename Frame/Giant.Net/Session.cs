using Giant.Log;
using Giant.Msg;
using Giant.Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Giant.Net
{
    public class Session : IDisposable
    {
        private readonly BaseChannel channel;//通讯对象
		private readonly byte[] opcodeBytes = new byte[2];
        private readonly Dictionary<int, Action<IResponse>> responseCallback = new Dictionary<int, Action<IResponse>>();//消息回调

        public NetworkService NetworkService { get; private set; }

        public long Id { get; private set; }

        private int RpcId { get; set; }

        public Session(NetworkService networkService, BaseChannel baseChannel)
        {
            Id = baseChannel.InstanceId;
            NetworkService = networkService;
            this.channel = baseChannel;

            this.channel.OnReadCallback += OnRead;
            this.channel.OnErrorCallback += OnError;
        }

        public void Reply(IMessage message)
        {
            this.Send(message);
        }

        public void Send(IMessage message)
        {
            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(message.GetType());
            this.Send(opcode, message);
        }

        private void Send(ushort opcode, IMessage message)
        {
            var stream = this.channel.Stream;
            opcodeBytes.WriteTo(0, opcode);

            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(opcodeBytes, 0, opcodeBytes.Length);
            stream.SetLength(Packet.MessageIndex);

            ProtoHelper.ToStream(stream, message);
            stream.Seek(0, SeekOrigin.Begin);

            this.channel.Send(stream);
        }

        public Task<IResponse> Call(IRequest message)
        {
            int rpcId = ++RpcId;
            message.RpcId = rpcId;

            ushort opcode = NetworkService.MessageDispatcher.GetOpcode(message.GetType());

            TaskCompletionSource<IResponse> tcs = new TaskCompletionSource<IResponse>();

            this.responseCallback[rpcId] = (response) =>
            {
                try
                {
                    tcs.SetResult(response);

                    //不能以异常的形式返回，客户端需要更具具体的错误码来做相应的操作
                    //if (response.Error == ErrorCode.ERR_Success)
                    //{
                    //    tcs.SetResult(response);
                    //}
                    //else
                    //{
                    //    tcs.SetException(new Exception($"ErrorCode {response.Error} Message {response.Message}"));
                    //}
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            };

            this.Send(opcode, message);

            return tcs.Task;
        }

        public void Start()
        {
            this.channel.Start();
        }

        public void Dispose()
        {
            channel.Dispose();

            //清空所有消息回调
            responseCallback.Clear();
        }

        private void OnRead(MemoryStream memoryStream)
        {
            //消息id
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);
            memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);

            Type msgType = this.NetworkService.MessageDispatcher.GetMessageType(opcode);
            IMessage message = this.NetworkService.MessageParser.DeserializeFrom(memoryStream, msgType) as IMessage;

            if (message is IResponse response)
            {
                if (responseCallback.TryGetValue(response.RpcId, out var action))
                {
                    action(response);
                    responseCallback.Remove(response.RpcId);
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

            NetworkService.Remove(this.Id);
        }

    }
}
