using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class UdpChannel : BaseChannel
    {
        private DateTime LastReceiveMsgTime = TimeHelper.Now;

        private Queue<byte[]> cachedMessage = new Queue<byte[]>();


        public uint RemoteUdp { get; set; }

        public IPEndPoint RemoteIPEndPoint { get; set; }

        public UdpChannel(uint udpId, Socket socket, IPEndPoint endPoint, UdpService udpService)
            : base(udpId, udpService, ChannelType.Connecter)
        {
            RemoteUdp = 0;
            this.Socket = socket;
            RemoteIPEndPoint = endPoint;

            this.Connect();
        }

        public UdpChannel(uint udpId, uint remoteUdpId, Socket socket, IPEndPoint endPoint, UdpService udpService)
            : base(udpId, udpService, ChannelType.Accepter)
        {
            RemoteUdp = remoteUdpId;
            this.Socket = socket;
            RemoteIPEndPoint = endPoint;

            this.Accept();
        }

        public void OnConnected(uint remoteId)
        {
            this.RemoteUdp = remoteId;
            this.IsConnected = true;
        }

        public void OnReceive(byte[] message, int offset, int length)
        {
            if (!IsConnected)
            {
                IsConnected = true;
            }

            cachedMessage.Enqueue(message.Read(offset, length));

            Console.WriteLine(message.ToUtf8String(offset, length));
        }

        public override void Transfer(byte[] message)
        {
            if (!IsConnected)
            {
                return;
            }

            byte[] content = new byte[message.Length + 9];
            content.WriteTo(0, UdpChannelState.MSG);
            content.WriteTo(1, Id);
            content.WriteTo(5, RemoteUdp);
            content.WriteTo(9, message);

            Write(content);
        }

        /// <summary>
        /// 模拟建立连接
        /// </summary>
        public override void Connect()
        {
            byte[] content = new byte[5];
            content.WriteTo(0, UdpChannelState.SYN);
            content.WriteTo(1, Id);

            Write(content);
        }

        public override void Update()
        {
            if (this.ChannelType == ChannelType.Connecter)
            {
                if (!IsConnected && (TimeHelper.Now - LastReceiveMsgTime).TotalSeconds > 10)
                {
                    this.Connect();
                }
            }

            while (cachedMessage.TryDequeue(out byte[] message))
            {
                Read(message);
            }
        }

        public override void Dispose()
        {
            this.IsConnected = false;

            byte[] content = new byte[9];
            content.WriteTo(0, UdpChannelState.FIN);
            content.WriteTo(1, Id);
            content.WriteTo(5, RemoteUdp);
            Write(content);
        }


        protected void Write(byte[] content)
        {
            try
            {
                Socket.SendTo(content, RemoteIPEndPoint);
            }
            catch
            {
                this.IsConnected = false;
            }
        }

        /// <summary>
        /// 回应消息.已经连接上了
        /// </summary>
        private void Accept()
        {
            byte[] content = new byte[9];
            content.WriteTo(0, UdpChannelState.ACK);
            content.WriteTo(1, Id);
            content.WriteTo(5, RemoteUdp);

            Write(content);
        }

    }
}
