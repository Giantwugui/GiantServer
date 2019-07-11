using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Giant.Share;
using Giant.Log;

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

        public override MemoryStream Stream => this.memoryStream;

        public TcpChannel(int packetSize, Socket socket, TcpService service):base(service, ChannelType.Accepter)
        {
            this.socket = socket;
            this.IsConnected = true;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            this.packetSizeCache = new byte[service.PacketSizeLength];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.parser = new PacketPacker(packetSize, this.recvBuffer, this.memoryStream);
        }

        public TcpChannel(int packetSize, IPEndPoint endPoint, TcpService service) : base(service, ChannelType.Connecter)
        {
            this.IsConnected = false;
            this.IPEndPoint = endPoint;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            this.packetSizeCache = new byte[service.PacketSizeLength];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.parser = new PacketPacker(packetSize, this.recvBuffer, this.memoryStream);

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            if (!this.IsConnected)
            {
                Connect();
            }
            else
            {
                if (!this.IsRecving)
                {
                    this.IsRecving = true;
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
            if (!IsConnected)
            {
                return;
            }

            switch (((TcpService)Service).PacketSizeLength)
            {
                case Packet.PacketSizeLength2:
                    {
                        if (stream.Length > ushort.MaxValue)
                        {
						    throw new Exception($"send packet too large: {stream.Length}");
                        }

                        this.packetSizeCache.WriteTo(0, (ushort)stream.Length);
                    }
                    break;
                case Packet.PacketSizeLength4:
                    {
                        if (stream.Length > ushort.MaxValue * 16)
                        {
                            throw new Exception($"send packet too large: {stream.Length}");
                        }

                        this.packetSizeCache.WriteTo(0, (int)stream.Length);
                    }
                    break;
            }

            this.sendBuffer.Write(packetSizeCache, 0, packetSizeCache.Length);
            this.sendBuffer.Write(stream);

            StartSend();
        }

        public override void Update()
        {
            if (!this.IsSending)
            {
            }
        }

        public override void Dispose()
        {
            this.socket.Close();
        }

        protected override void OnError(object error)
        {
            base.OnError(error);
            SetConnectState(false);
        }

        private void SetConnectState(bool state)
        {
            this.IsConnected = state;
            base.OnConnected(state);
        }

        private void ConnectAsync()
        {
            if (socket == null || this.IsConnecting)
            {
                return;
            }

            this.IsConnecting = true;
            innerArgs.RemoteEndPoint = IPEndPoint;
            if (this.socket.ConnectAsync(innerArgs))
            {
                return;
            }

            ConnectComplete(innerArgs);
        }

        private void StartSend()
        {
            if (!this.IsConnected)
            {
                return;
            }

            // 没有数据需要发送
            if (this.sendBuffer.Length == 0)
            {
                this.IsSending = false;
                return;
            }

            this.IsSending = true;

            int sendSize = this.sendBuffer.ChunkSize - this.sendBuffer.FirstIndex;
            if (sendSize > this.sendBuffer.Length)
            {
                sendSize = (int)this.sendBuffer.Length;
            }

            this.SendAsync(this.sendBuffer.First, this.sendBuffer.FirstIndex, sendSize);
        }

        private void SendAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                this.innerArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }
            if (this.socket.SendAsync(this.innerArgs))
            {
                return;
            }
            SendComplete(this.innerArgs);
        }

        private void StartRecv()
        {
            int size = recvBuffer.ChunkSize - recvBuffer.LastIndex;

            ReceiveAsync(recvBuffer.Last, recvBuffer.LastIndex, size);
        }

        private void ReceiveAsync(byte[] buffer, int offset, int length)
        {
            outtererArgs.SetBuffer(buffer, offset, length);
            if (socket.ReceiveAsync(outtererArgs))
            {
                return;
            }

            ReceiveComplete(outtererArgs);
        }

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ConnectComplete(eventArgs);
                    break;
                case SocketAsyncOperation.Receive:
                    ReceiveComplete(eventArgs);
                    break;
                case SocketAsyncOperation.Send:
                    SendComplete(eventArgs);
                    break;
            }
        }

        private void ConnectComplete(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs.SocketError == SocketError.Success)
            {
                this.IsConnected = true;

                Start();
                SetConnectState(true);
            }
            else
            {
                this.OnError(eventArgs.SocketError);
            }
            this.IsConnecting = false;
        }

        private void ReceiveComplete(SocketAsyncEventArgs eventArgs)
        {
            if (this.socket == null)
            {
                return;
            }

            SocketAsyncEventArgs e = (SocketAsyncEventArgs)eventArgs;

            if (e.SocketError != SocketError.Success)
            {
                this.OnError(e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.PeerDisconnect);
                return;
            }

            this.recvBuffer.LastIndex += e.BytesTransferred;
            if (this.recvBuffer.LastIndex == this.sendBuffer.ChunkSize)
            {
                this.recvBuffer.LastIndex = 0;
                this.recvBuffer.AddLast();
            }

            // 收到消息回调
            while (true)
            {
                try
                {
                    if (!this.parser.Parse())
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    this.OnError(ErrorCode.SocketError);
                    return;
                }

                try
                {
                    this.OnRead(this.parser.GetPacket());
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            StartRecv();
        }

        private void SendComplete(SocketAsyncEventArgs eventArgs)
        {
            if (this.socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)eventArgs;

            if (e.SocketError != SocketError.Success)
            {
                this.OnError(e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.PeerDisconnect);
                return;
            }

            this.sendBuffer.FirstIndex += e.BytesTransferred;
            if (this.sendBuffer.FirstIndex == this.sendBuffer.ChunkSize)
            {
                this.sendBuffer.FirstIndex = 0;
                this.sendBuffer.RemoveFirst();
            }

            this.StartSend();
        }

    }
}
