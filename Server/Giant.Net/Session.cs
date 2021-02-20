using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Net
{
    public class Session : Entity, IInitSystem<BaseChannel>
    {
        private int rpcId;
        private BaseChannel channel;//通讯对象
        private readonly byte[] opcodeBytes = new byte[2];
        private readonly Dictionary<int, Action<IResponse>> responseCallback = new Dictionary<int, Action<IResponse>>();//消息回调

        public NetworkComponent NetworkComponent => GetParent<NetworkComponent>();

        public long Id { get; private set; }

        public bool IsConnected => channel.IsConnected;
        public IPEndPoint RemoteIPEndPoint => channel.IPEndPoint;

        private Action<Session, bool> onConnectCallback;
        public event Action<Session, bool> OnConnectCallback
        {
            add { onConnectCallback += value; }
            remove { onConnectCallback -= value; }
        }

        public bool IsNeedEncrypt { get; set; }
        public AESCrypt AESCrypt { get; private set; }
        public string SecretKey => AESCrypt.EncryptKey;

        public void Init(BaseChannel baseChannel)
        {
            Id = baseChannel.InstanceId;
            channel = baseChannel;

            channel.OnReadCallback += OnRead;
            channel.OnErrorCallback += OnError;
            channel.OnConnectCallback += OnConnect;

            AESCrypt = new AESCrypt();
        }

        public void Reply(IMessage message)
        {
            Notify(message);
        }

        public void Notify(IMessage message)
        {
            ushort opcode = OpcodeComponent.Instance.GetOpcode(message.GetType());
            Notify(opcode, message);
        }

        public Task<IResponse> Call(IRequest request)
        {
            request.RpcId = ++rpcId;

            ushort opcode = OpcodeComponent.Instance.GetOpcode(request.GetType());

            TaskCompletionSource<IResponse> tcs = new TaskCompletionSource<IResponse>();

            responseCallback[rpcId] = (response) =>
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

            Notify(opcode, request);

            return tcs.Task;
        }

        public Task<IResponse> Call(IRequest request, CancellationToken cancellation)
        {
            request.RpcId = ++rpcId;

            ushort opcode = OpcodeComponent.Instance.GetOpcode(request.GetType());

            TaskCompletionSource<IResponse> tcs = new TaskCompletionSource<IResponse>();

            responseCallback[rpcId] = (response) =>
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

            cancellation.Register(() =>
            {
                responseCallback.Remove(rpcId);
                Log.Warn($"session call OOT request {rpcId} type {request.GetType().Name}");
            });

            Notify(opcode, request);

            return tcs.Task;
        }

        public void Start()
        {
            channel.Start();
        }

        public void Connect()
        {
            channel.Connect();
        }

        public override void Dispose()
        {
            base.Dispose();
            channel.Dispose();

            //清空所有消息回调
            responseCallback.Clear();
        }

        private void Notify(ushort opcode, IMessage message)
        {
            var stream = channel.Stream;
            opcodeBytes.WriteTo(0, opcode);

            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(opcodeBytes, 0, opcodeBytes.Length);
            stream.SetLength(Packet.MessageIndex);

            if (IsNeedEncrypt)
            {
                byte[] encrypt = AESCrypt.Encrypt(ProtoHelper.ToBytes(message));
                stream.Write(encrypt, 0, encrypt.Length);
            }
            else
            { 
                ProtoHelper.ToStream(stream, message);
            }

            stream.Seek(0, SeekOrigin.Begin);

            channel.Send(stream);
        }

        private void OnRead(MemoryStream memoryStream)
        {
            //消息id
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);
            memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);

            IMessage message;
            Type msgType = OpcodeComponent.Instance.GetMessageType(opcode);

            if (!IsNeedEncrypt || opcode == OuterOpcode.Msg_CG_Get_SecretKey)
            {
                message = NetworkComponent.MessageParser.DeserializeFrom(memoryStream, msgType) as IMessage;
            }
            else
            {
                byte[] byteList = new byte[memoryStream.Length - Packet.PacketSizeLength2];
                Array.Copy(memoryStream.GetBuffer(), Packet.PacketSizeLength2, byteList, 0, byteList.Length);

                byte[] decode = AESCrypt.Decrypt(byteList);
                message = NetworkComponent.MessageParser.DeserializeFrom(decode, msgType) as IMessage;
            }

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
                MessageDispatcherComponent.Instance.Dispatch(this, opcode, message);
            }
        }

        public void OnError(object error)
        {
            switch (error)
            {
                case int errorCode:
                    Log.Error($"ErrorCode {errorCode}");
                    break;
                case SocketError socketError:
                    Log.Error($"SocketError {socketError}");
                    break;
                default:
                    Log.Error(error);
                    break;
            }

            NetworkComponent.Remove(this);
        }

        public void OnConnect(bool connState)
        {
            onConnectCallback?.Invoke(this, connState);
        }

    }
}
