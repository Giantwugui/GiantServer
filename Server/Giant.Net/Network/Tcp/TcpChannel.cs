using Giant.Core;
using Giant.Util;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Giant.Logger;

namespace Giant.Net
{
    public class TcpChannel : BaseChannel
    {
        private readonly Socket socket;
        private readonly byte[] packetSizeCache;

        private readonly PacketPacker parser;
        private readonly MemoryStream memoryStream;
        private readonly CircularBuffer recvBuffer = new();//接收消息临时缓冲区
        private readonly CircularBuffer sendBuffer = new();//发送消息临时缓冲区

        private readonly SocketAsyncEventArgs innerArgs = new();
        private readonly SocketAsyncEventArgs outerArgs = new();

        private bool isSending;
        private bool isConnecting;
        private bool isRecving;

        public override MemoryStream Stream => memoryStream;

        public TcpChannel(Socket socket, TcpService service) : base(service, ChannelType.Accepter)
        {
            this.socket = socket;
            IsConnected = true;

            innerArgs.Completed += OnComplete;
            outerArgs.Completed += OnComplete;

            packetSizeCache = new byte[Packet.PacketSizeLength];
            memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            parser = new PacketPacker(recvBuffer, memoryStream);
        }

        public TcpChannel(IPEndPoint endPoint, TcpService service) : base(service, ChannelType.Connecter)
        {
            IsConnected = false;
            IPEndPoint = endPoint;

            innerArgs.Completed += OnComplete;
            outerArgs.Completed += OnComplete;

            packetSizeCache = new byte[Packet.PacketSizeLength];
            memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            parser = new PacketPacker(recvBuffer, memoryStream);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.NoDelay = true;
        }

        public override void Start()
        {
            if (!IsConnected)
            {
                Connect();
            }
            else
            {
                if (!isRecving)
                {
                    isRecving = true;
                    StartRecv();
                }
            }
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        public override void Connect()
        {
            ConnectAsync();
        }

        public override void Send(MemoryStream stream)
        {
            if (IsDisposed)
            {
                Log.Error("can not send message with disposed object");
                return;
            }

            if (!IsConnected) return;

            if (stream.Length > ushort.MaxValue * 16)
            {
                throw new Exception($"send packet too large: {stream.Length}");
            }

            packetSizeCache.WriteTo(0, (int)stream.Length);

            sendBuffer.Write(packetSizeCache, 0, packetSizeCache.Length);
            sendBuffer.Write(stream);

            StartSend();
        }

        public override void Update()
        {
            if (!isSending)
            {
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            socket.Dispose();
        }

        protected override void OnError(object error)
        {
            base.OnError(error);
            SetConnectState(false);
        }

        private void SetConnectState(bool state)
        {
            IsConnected = state;
            base.OnConnected(state);
        }

        private void ConnectAsync()
        {
            if (socket == null || isConnecting)
            {
                return;
            }

            isConnecting = true;
            innerArgs.RemoteEndPoint = IPEndPoint;
            if (socket.ConnectAsync(innerArgs))
            {
                return;
            }

            ConnectComplete(innerArgs);
        }

        private void StartSend()
        {
            if (IsDisposed) return;
            if (!IsConnected) return;

            // 没有数据需要发送
            if (sendBuffer.Length == 0)
            {
                isSending = false;
                return;
            }

            isSending = true;

            int sendSize = sendBuffer.ChunkSize - sendBuffer.FirstIndex;
            if (sendSize > sendBuffer.Length)
            {
                sendSize = (int)sendBuffer.Length;
            }

            SendAsync(sendBuffer.First, sendBuffer.FirstIndex, sendSize);
        }

        private void SendAsync(byte[] buffer, int offset, int count)
        {
            if (IsDisposed) return;

            try
            {
                innerArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }

            try
            {
                if (socket.SendAsync(innerArgs))
                {
                    return;
                }
                SendComplete(innerArgs);
            }
            catch (Exception)
            {
                OnError(ErrorCode.SocketCantSend);
            }
        }

        private void StartRecv()
        {
            int size = recvBuffer.ChunkSize - recvBuffer.LastIndex;

            ReceiveAsync(recvBuffer.Last, recvBuffer.LastIndex, size);
        }

        private void ReceiveAsync(byte[] buffer, int offset, int length)
        {
            if (IsDisposed) return;
            try
            {
                outerArgs.SetBuffer(buffer, offset, length);
                if (socket.ReceiveAsync(outerArgs))
                {
                    return;
                }

                ReceiveComplete(outerArgs);
            }
            catch (Exception)
            {
                OnError(ErrorCode.SocketError);
            }
        }

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    //ConnectComplete(eventArgs);
                    OneThreadSynchronizationContext.Instance.Post(ConnectComplete, eventArgs);
                    break;
                case SocketAsyncOperation.Receive:
                    //ReceiveComplete(eventArgs);
                    OneThreadSynchronizationContext.Instance.Post(ReceiveComplete, eventArgs);
                    break;
                case SocketAsyncOperation.Send:
                    //SendComplete(eventArgs);
                    OneThreadSynchronizationContext.Instance.Post(SendComplete, eventArgs);
                    break;
            }
        }

        private void ConnectComplete(object eventArgs)
        {
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)eventArgs;
            if (e.SocketError == SocketError.Success)
            {
                IsConnected = true;

                Start();
                SetConnectState(true);
            }
            else
            {
                OnError(e.SocketError);
            }
            isConnecting = false;
        }

        private void ReceiveComplete(object eventArgs)
        {
            if (socket == null)
            {
                return;
            }

            SocketAsyncEventArgs e = (SocketAsyncEventArgs)eventArgs;

            if (e.SocketError != SocketError.Success)
            {
                OnError(e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                OnError(ErrorCode.PeerDisconnect);
                return;
            }

            recvBuffer.LastIndex += e.BytesTransferred;
            if (recvBuffer.LastIndex == sendBuffer.ChunkSize)
            {
                recvBuffer.LastIndex = 0;
                recvBuffer.AddLast();
            }

            // 收到消息回调
            while (true)
            {
                try
                {
                    if (!parser.Parse())
                    {
                        break;
                    }

                    OnRead(parser.GetPacket());
                }
                catch (Exception ex)
                {
                    OnError(ex);
                    return;
                }
            }

            StartRecv();
        }

        private void SendComplete(object eventArgs)
        {
            if (socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)eventArgs;

            if (e.SocketError != SocketError.Success)
            {
                OnError(e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                OnError(ErrorCode.PeerDisconnect);
                return;
            }

            sendBuffer.FirstIndex += e.BytesTransferred;
            if (sendBuffer.FirstIndex == sendBuffer.ChunkSize)
            {
                sendBuffer.FirstIndex = 0;
                sendBuffer.RemoveFirst();
            }

            StartSend();
        }

    }
}
