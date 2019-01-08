using System;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    abstract class BaseChannel
    {
        public BaseChannel(IPEndPoint endPoint)
        {
            RemoteAddress = endPoint;
        }

        public BaseChannel(Socket socket)
        {
            RemoteAddress = (IPEndPoint)socket.RemoteEndPoint;
        }


        public abstract void Start();
        public abstract void Send();

        protected void OnError(SocketError socketError)
        {
            mErrorCallBack?.Invoke(this, socketError);
        }

        private Action<BaseChannel, SocketError> mErrorCallBack;
        public event Action<BaseChannel, SocketError> ErrorCallBack
        {
            add { mErrorCallBack += value; }
            remove { mErrorCallBack -= value; }
        }

        protected void OnReceived(byte[] message)
        {
            mReceiveCallBack?.Invoke(message);
        }

        private Action<byte[]> mReceiveCallBack;
        public event Action<byte[]> ReceiveCallBack
        {
            add { mReceiveCallBack += value; }
            remove { mReceiveCallBack -= value; }
        }


        public long Id { get; private set; }

        public IPEndPoint RemoteAddress { get; set; }
    }
}
