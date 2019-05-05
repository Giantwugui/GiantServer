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
        private readonly IPEndPoint ipEndPoint;
        private readonly byte[] packetSizeCache;

        private readonly PacketParser parser;
        private readonly MemoryStream memoryStream;
        private readonly CircularBuffer recvBuffer = new CircularBuffer();//接收消息临时缓冲区
        private readonly CircularBuffer sendBuffer = new CircularBuffer();//发送消息临时缓冲区

        private readonly SocketAsyncEventArgs innerArgs = new SocketAsyncEventArgs();
        private readonly SocketAsyncEventArgs outtererArgs = new SocketAsyncEventArgs();


        public bool IsSending { get; set; }
        public bool IsRecving { get; set; }

        public override MemoryStream Stream => this.memoryStream;

        public TcpChannel(int packetSize, Socket socket, TcpService service):base(service, ChannelType.Accepter)
        {
            this.socket = socket;
            this.IsSending = false;
            this.IsConnected = true;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            this.packetSizeCache = new byte[service.PacketSizeLength];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.parser = new PacketParser(packetSize, this.recvBuffer, this.memoryStream);
        }

        public TcpChannel(int packetSize, IPEndPoint endPoint, TcpService service) : base(service, ChannelType.Connecter)
        {
            this.IsSending = false;
            this.IsConnected = false;
            this.ipEndPoint = endPoint;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            this.packetSizeCache = new byte[service.PacketSizeLength];
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.parser = new PacketParser(packetSize, this.recvBuffer, this.memoryStream);

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            if (ChannelType == ChannelType.Connecter)
            {
                Connect();
            }
            else
            {
                StartRecv();
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
                        if (stream.Length > uint.MaxValue)
                        {
						    throw new Exception($"send packet too large: {stream.Length}");
                        }

                        this.packetSizeCache.WriteTo(0, (ushort)stream.Length);
                    }
                    break;
                case Packet.PacketSizeLength4:
                    {
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
            this.IsConnected = false;
        }

        protected override void OnError(object error)
        {
            if (this.IsConnected)
            {
                this.IsConnected = false;
            }

            base.OnError(error);
        }

        private void ConnectAsync()
        {
            try
            {
                if (socket == null)
                {
                    return;
                }

                innerArgs.RemoteEndPoint = ipEndPoint;
                if (this.socket.ConnectAsync(innerArgs))
                {
                    return;
                }

                ConnectComplete(innerArgs);
            }
            catch (Exception ex)
            {
                this.OnError(SocketError.ConnectionRefused);
                this.OnError(ex);
            }
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
                this.outtererArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }
            if (this.socket.SendAsync(this.outtererArgs))
            {
                return;
            }
            SendComplete(this.outtererArgs);
        }

        private void StartRecv()
        {
            int size = recvBuffer.ChunkSize - recvBuffer.LastIndex;

            ReceiveAsync(recvBuffer.Last, recvBuffer.LastIndex, size);
        }

        private void ReceiveAsync(byte[] buffer, int offset, int length)
        {
            innerArgs.SetBuffer(buffer, offset, length);
            if (socket.ReceiveAsync(innerArgs))
            {
                return;
            }

            ReceiveComplete(innerArgs);
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
            if (eventArgs.LastOperation == SocketAsyncOperation.Connect && eventArgs.SocketError == SocketError.Success)
            {
                this.IsConnected = true;

                StartRecv();
            }
            else
            {
                this.OnError(eventArgs.SocketError);
            }
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
                this.OnError((int)e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.ERR_PeerDisconnect);
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
                catch (Exception ee)
                {
                    Logger.Error(ee);
                    this.OnError(ErrorCode.ERR_SocketError);
                    return;
                }

                try
                {
                    this.OnRead(this.parser.GetPacket());
                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
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
                this.OnError((int)e.SocketError);
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.ERR_PeerDisconnect);
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
