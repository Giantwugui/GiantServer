using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class TCPChannel:BChannel
    {
        public TCPChannel(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            IsConnected = false;
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mSocket.NoDelay = true;

            RemoteAddress = ipEndPoint;

            mInnerArgs.Completed += OnComplete;
            mOuterArgs.Completed += OnComplete;
        }

        public TCPChannel(Socket socket) : base(socket)
        {
            IsConnected = true;
            mSocket = socket;
            mSocket.NoDelay = true;

            mInnerArgs.Completed += OnComplete;
            mOuterArgs.Completed += OnComplete;
        }

        public override void Start()
        {
            if (!IsConnected)
            {
                ConnAsync();
                return;
            }

            ReceiveAsync();
        }

        public override void Send(string message)
        {
            SendAsync();
        }

        #region 异步方法

        private void ConnAsync()
        {
            mOuterArgs.RemoteEndPoint = RemoteAddress;

            if (mSocket.ConnectAsync(mInnerArgs))
            {
                return;
            }

            OnConnComplete(mOuterArgs);
        }

        private void ReceiveAsync()
        {
            if(mSocket.ReceiveAsync(mInnerArgs))
            {
                return;
            }

            OnReceiveComplate(mInnerArgs);
        }

        private void SendAsync()
        {
            if (mSocket.SendAsync(mInnerArgs))
            {
                return;
            }

            OnSendComplate(mInnerArgs);
        }

        #endregion

        #region 异步回调

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    OnConnComplete(eventArgs);
                    break;
                case SocketAsyncOperation.Receive:
                    OnReceiveComplate(eventArgs);
                    break;
                case SocketAsyncOperation.Send:
                    OnSendComplate(eventArgs);
                    break;
                case SocketAsyncOperation.Disconnect:
                    OnDisconnectComplete(eventArgs);
                    break;

            }
        }


        private void OnConnComplete(SocketAsyncEventArgs eventArgs)
        {
            if (mSocket == null) return;

            if (eventArgs.SocketError != SocketError.Success)
            {
                OnError(eventArgs.SocketError);
            }

            eventArgs.RemoteEndPoint = null;

            IsConnected = true;

            Start();
        }

        private void OnDisconnectComplete(SocketAsyncEventArgs eventArgs)
        {
            if (mSocket == null) return;

            OnError(eventArgs.SocketError);
        }

        private void OnReceiveComplate(SocketAsyncEventArgs eventArgs)
        {
            if (mSocket == null) return;

            if (eventArgs.SocketError != SocketError.Success)
            {
                OnError(eventArgs.SocketError);
            }

            ReceiveAsync();
        }


        private void OnSendComplate(SocketAsyncEventArgs eventArgs)
        {
            if (mSocket == null) return;

            if (eventArgs.SocketError != SocketError.Success)
            {
                OnError(eventArgs.SocketError);
            }
        }

        #endregion


        private bool IsConnected;

        private Socket mSocket;

        private readonly SocketAsyncEventArgs mInnerArgs = new SocketAsyncEventArgs();
        private readonly SocketAsyncEventArgs mOuterArgs = new SocketAsyncEventArgs();
    }
}
