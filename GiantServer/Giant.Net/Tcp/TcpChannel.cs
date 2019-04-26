using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace Giant.Net
{
    public class TcpChannel : BaseChannel
    {
        private const ushort lengthSize = 2;//消息长度所占字节数
        private const int headLength= 6;//消息头长度 lengthSize + messageId
        private const ushort contentLength = ushort.MaxValue;//最大发送消息长度

        private IPEndPoint ipEndPoint;
        private byte[] sendBuffer = new byte[contentLength];//发送消息临时缓冲区
        private byte[] receiveBuffer = new byte[contentLength];//接收消息临时缓冲区
        private ConcurrentQueue<byte[]> waitSendMessage = new ConcurrentQueue<byte[]>();//发送消息队列
        private ConcurrentQueue<byte[]> receivedMessage = new ConcurrentQueue<byte[]>();//接收消息队列

        private SocketAsyncEventArgs innerArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs outtererArgs = new SocketAsyncEventArgs();

        public TcpChannel(Socket socket, TcpService service):base(service, ChannelType.Accepter)
        {
            this.Socket = socket;
            this.IsConnected = true;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;
        }

        public TcpChannel(IPEndPoint endPoint, TcpService service) : base(service, ChannelType.Connecter)
        {
            this.IsConnected = false;
            this.ipEndPoint = endPoint;

            innerArgs.Completed += OnComplete;
            outtererArgs.Completed += OnComplete;

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            if (ChannelType == ChannelType.Connecter)
            {
                Connect();
            }
            else
            {
                ReceiveAsync();
            }
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        public override void Connect()
        {
            ConnectAsync();
        }

        public override void Transfer(byte[] message)
        {
            if (!IsConnected)
            {
                return;
            }

            byte[] content = new byte[message.Length + lengthSize];

            content.WriteTo(0, (ushort)message.Length);
            content.WriteTo(2, message);

            waitSendMessage.Enqueue(content);
        }

        public override void Update()
        {
            SendAsync();
        }

        public override void Dispose()
        {
            this.IsConnected = false;
        }

        protected override void Error(object error)
        {
            if (this.IsConnected)
            {
                this.IsConnected = false;
            }

            base.Error(error);
        }

        private void ConnectAsync()
        {
            try
            {
                if (Socket == null)
                {
                    return;
                }

                innerArgs.RemoteEndPoint = ipEndPoint;
                if (this.Socket.ConnectAsync(innerArgs))
                {
                    return;
                }

                ConnectComplete(innerArgs);
            }
            catch (Exception ex)
            {
                this.Error(SocketError.ConnectionRefused);
                this.Error(ex);
            }
        }

        private void SendAsync()
        {
            try
            {
                if (waitSendMessage.TryDequeue(out byte[] message))
                {
                    outtererArgs.SetBuffer(message);

                    if (Socket.SendAsync(outtererArgs))
                    {
                        return;
                    }

                    SendComplete(outtererArgs);
                }
            }
            catch (Exception ex)
            {
                this.Error(SocketError.Disconnecting);
                this.Error(ex);
            }
        }

        private void ReceiveAsync()
        {
            try
            {
                innerArgs.SetBuffer(receiveBuffer, 0, contentLength);
                if (Socket.ReceiveAsync(innerArgs))
                {
                    return;
                }

                ReceiveComplete(innerArgs);
            }
            catch (Exception ex)
            {
                Error(SocketError.ConnectionReset);
                Error(ex);
            }
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

                ReceiveAsync();
            }
            else
            {
                this.Error(eventArgs.SocketError);
            }
        }

        private void ReceiveComplete(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs.BytesTransferred > 0 && eventArgs.SocketError == SocketError.Success)
            {
                byte[] message = new byte[eventArgs.BytesTransferred];

                Array.Copy(eventArgs.Buffer, message, eventArgs.BytesTransferred);

                this.Read(message);

                ReceiveAsync();
            }
            else
            {
                this.Error(SocketError.SocketError);
            }
        }

        private void SendComplete(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs.BytesTransferred > 0 && eventArgs.SocketError == SocketError.Success)
            {
                SendAsync();
            }
            else
            {
                this.Error(SocketError.SocketError);
            }
        }

    }
}
