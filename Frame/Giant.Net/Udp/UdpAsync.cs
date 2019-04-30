using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class UdpAsync
    {
        private Socket socket;
        private static ushort contentLength = ushort.MaxValue;//最大发送消息长度

        /// <summary>
        /// 远端端口
        /// </summary>
        private IPEndPoint remoteIPEndPoint;

        public bool IsClient { get; set; }

        public IPEndPoint IPEndPoint { get; private set; }


        public Action<byte[], IPEndPoint> OnReceived { get; set; }

        SocketAsyncEventArgs innerArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs outterArgs = new SocketAsyncEventArgs();

        public UdpAsync(int port)
        {
            IsClient = false;

            IPEndPoint = new IPEndPoint(IPAddress.Any, port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(IPEndPoint);

            innerArgs.Completed += OnComplate;
            outterArgs.Completed += OnComplate;
        }

        public UdpAsync(string ip, int port)
        {
            IsClient = true;

            remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            innerArgs.Completed += OnComplate;
            outterArgs.Completed += OnComplate;
        }


        public void Start()
        {
            if (IsClient)
            {
                ConnectAsync();
            }
            else
            {
                ReceiveAsync();
            }
        }

        public void Send(byte[] content, IPEndPoint endPoint = null)
        {
            SendAsync(content, IsClient ? remoteIPEndPoint : endPoint);
        }

        private void ConnectAsync()
        {
            try
            {
                innerArgs.RemoteEndPoint = remoteIPEndPoint;

                if (socket.ConnectAsync(innerArgs))
                {
                    return;
                }

                OnConnectComplate(innerArgs);
            }
            catch
            {
            }
        }

        private void ReceiveAsync()
        {
            try
            {
                byte[] receiveBuffer = new byte[contentLength];
                innerArgs.SetBuffer(receiveBuffer);
                innerArgs.RemoteEndPoint = IsClient ? remoteIPEndPoint : IPEndPoint;

                if (socket.ReceiveFromAsync(innerArgs))
                {
                    return;
                }

                OnReceiveComplate(innerArgs);
            }
            catch
            {
            }
        }

        private void SendAsync(byte[] content, IPEndPoint endPoint)
        {
            try
            {
                outterArgs.SetBuffer(content);
                outterArgs.RemoteEndPoint = remoteIPEndPoint;

                if (socket.SendToAsync(outterArgs))
                {
                    return;
                }

                OnSendComplate(outterArgs);
            }
            catch
            {
            }
        }

        private void OnComplate(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    OnConnectComplate(eventArgs);
                    break;
                case SocketAsyncOperation.ReceiveFrom:
                    OnReceiveComplate(eventArgs);
                    break;
                case SocketAsyncOperation.SendTo:
                    OnSendComplate(eventArgs);
                    break;
            }
        }

        private void OnReceiveComplate(SocketAsyncEventArgs eventArgs)
        {
            try
            {
                if (eventArgs.SocketError == SocketError.Success && eventArgs.BytesTransferred > 0)
                {
                    this.remoteIPEndPoint = (IPEndPoint)eventArgs.RemoteEndPoint;

                    byte[] content = new byte[eventArgs.BytesTransferred];

                    Array.Copy(eventArgs.Buffer, content, eventArgs.BytesTransferred);

                    OnReceived(content, this.remoteIPEndPoint);

                    ReceiveAsync();
                }
            }
            catch
            {
            }
        }


        private void OnConnectComplate(SocketAsyncEventArgs eventArgs)
        {
            try
            {
                if (eventArgs.SocketError == SocketError.Success)
                {
                    ReceiveAsync();
                }
                else
                {
                }
            }
            catch
            {
            }
        }

        private void OnSendComplate(SocketAsyncEventArgs eventArgs)
        {
            try
            {
                if (eventArgs.SocketError == SocketError.Success && eventArgs.BytesTransferred > 0)
                {
                }
                else
                {
                }
            }
            catch
            {
            }
        }
    }
}
