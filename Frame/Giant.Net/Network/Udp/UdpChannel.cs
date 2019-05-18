using Giant.Share;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class UdpChannel : BaseChannel
    {
        private readonly Socket socket;
        private readonly MemoryStream sendStream;
        private readonly DateTime LastReceiveMsgTime = TimeHelper.Now;

        public uint RemoteUdp { get; set; }

        public IPEndPoint RemoteIPEndPoint { get; set; }
        public override MemoryStream Stream => this.sendStream;

        public UdpChannel(uint udpId, Socket socket, IPEndPoint endPoint, UdpService service) : base(udpId, service, ChannelType.Connecter)
        {
            RemoteUdp = 0;
            this.socket = socket;
            RemoteIPEndPoint = endPoint;

            this.sendStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.Connect();
        }

        public UdpChannel(uint udpId, uint remoteUdpId, Socket socket, IPEndPoint endPoint, UdpService service) : base(udpId, service, ChannelType.Accepter)
        {
            this.socket = socket;
            RemoteUdp = remoteUdpId;
            RemoteIPEndPoint = endPoint;
            this.sendStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);

            this.Accept();
        }

        public void OnConnected(uint remoteId)
        {
            this.RemoteUdp = remoteId;
            this.IsConnected = true;
        }

        public void OnReceive(byte[] message, int offset, int length)
        {
            if (!this.IsConnected)
            {
                this.IsConnected = true;
            }

            this.sendStream.Seek(0, SeekOrigin.Begin);
            this.sendStream.Write(message, offset, length);

            OnRead(this.sendStream);
        }

        public override void Send(MemoryStream stream)
        {
            if (!IsConnected)
            {
                return;
            }

            byte[] content = new byte[stream.Length + 9];
            content.WriteTo(0, UdpChannelState.MSG);
            content.WriteTo(1, InstanceId);
            content.WriteTo(5, RemoteUdp);

            Array.Copy(stream.GetBuffer(), stream.Position, content, 9, stream.Length);

            SendTo(content);
        }

        /// <summary>
        /// 模拟建立连接
        /// </summary>
        public override void Connect()
        {
            byte[] content = new byte[5];
            content.WriteTo(0, UdpChannelState.SYN);
            content.WriteTo(1, InstanceId);

            SendTo(content);
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
        }

        public override void Dispose()
        {
            this.IsConnected = false;

            byte[] content = new byte[9];
            content.WriteTo(0, UdpChannelState.FIN);
            content.WriteTo(1, InstanceId);
            content.WriteTo(5, RemoteUdp);
            SendTo(content);
        }

        /// <summary>
        /// 回应消息.已经连接上了
        /// </summary>
        private void Accept()
        {
            byte[] content = new byte[9];
            content.WriteTo(0, UdpChannelState.ACK);
            content.WriteTo(1, InstanceId);
            content.WriteTo(5, RemoteUdp);

            SendTo(content);
        }


        protected void SendTo(byte[] content)
        {
            try
            {
                socket.SendTo(content, RemoteIPEndPoint);
            }
            catch
            {
                this.IsConnected = false;
            }
        }

    }
}
