using Giant.Core;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class TcpChannel : BaseChannel
    {
        private readonly Socket socket;
        private readonly byte[] packetSizeCache;

        private readonly PacketPacker parser;
        private readonly MemoryStream memoryStream;
        private readonly CircularBuffer recvBuffer = new CircularBuffer();//接收消息临时缓冲区
        private readonly CircularBuffer sendBuffer = new CircularBuffer();//发送消息临时缓冲区

        private readonly SocketAsyncEventArgs innerArgs = new SocketAsyncEventArgs();
        private readonly SocketAsyncEventArgs outtererArgs = new SocketAsyncEventArgs();


        public bool IsSending { get; private set; }
        public bool IsConnecting { get; private set; }
        public bool IsRecving { get; private set; }

        public override MemoryStream Stream => memoryStream;

        public TcpChannel(int packetSize, Socket socket, TcpService service) : base(service, ChannelType.Accepter)
        {
            this.socket = socket;
            IsConnected = true;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            packetSizeCache = new byte[service.PacketSizeLength];
            memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            parser = new PacketPacker(packetSize, recvBuffer, memoryStream);
        }

        public TcpChannel(int packetSize, IPEndPoint endPoint, TcpService service) : base(service, ChannelType.Connecter)
        {
            IsConnected = false;
            IPEndPoint = endPoint;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            packetSizeCache = new byte[service.PacketSizeLength];
            memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            parser = new PacketPacker(packetSize, recvBuffer, memoryStream);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            if (!IsConnected)
            {
                Connect();
            }
            else
            {
                if (!IsRecving)
                {
                    IsRecving = true;
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
            if (IsDisposed()) return;
            if (!IsConnected) return;

            switch (((TcpService)Service).PacketSizeLength)
            {
                case Packet.PacketSizeLength2:
                    {
                        if (stream.Length > ushort.MaxValue)
                        {
                            throw new Exception($"send packet too large: {stream.Length}");
                        }

                        packetSizeCache.WriteTo(0, (ushort)stream.Length);
                    }
                    break;
                case Packet.PacketSizeLength4:
                    {
                        if (stream.Length > ushort.MaxValue * 16)
                        {
                            throw new Exception($"send packet too large: {stream.Length}");
                        }

                        packetSizeCache.WriteTo(0, (int)stream.Length);
                    }
                    break;
            }

            sendBuffer.Write(packetSizeCache, 0, packetSizeCache.Length);
            sendBuffer.Write(stream);

            StartSend();
        }

        public override void Update()
        {
            if (!IsSending)
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
            if (socket == null || IsConnecting)
            {
                return;
            }

            IsConnecting = true;
            innerArgs.RemoteEndPoint = IPEndPoint;
            if (socket.ConnectAsync(innerArgs))
            {
                return;
            }

            ConnectComplete(innerArgs);
        }

        private void StartSend()
        {
            if (IsDisposed()) return;
            if (!IsConnected) return;

            // 没有数据需要发送
            if (sendBuffer.Length == 0)
            {
                IsSending = false;
                return;
            }

            IsSending = true;

            int sendSize = sendBuffer.ChunkSize - sendBuffer.FirstIndex;
            if (sendSize > sendBuffer.Length)
            {
                sendSize = (int)sendBuffer.Length;
            }

            SendAsync(sendBuffer.First, sendBuffer.FirstIndex, sendSize);
        }

        private void SendAsync(byte[] buffer, int offset, int count)
        {
            if (IsDisposed()) return;

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
            if (IsDisposed()) return;
            try
            {
                outtererArgs.SetBuffer(buffer, offset, length);
                if (socket.ReceiveAsync(outtererArgs))
                {
                    return;
                }

                ReceiveComplete(outtererArgs);
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
            IsConnecting = false;
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
