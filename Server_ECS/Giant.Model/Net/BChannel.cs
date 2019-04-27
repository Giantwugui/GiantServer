using Giant.Model;
using System;
using System.Net;
using System.Net.Sockets;

namespace Giant.Model
{
    public abstract class BChannel
    {
        public BChannel(IPEndPoint endPoint)
        {
            RemoteAddress = endPoint;
        }

        public BChannel(Socket socket)
        {
            Id = IdGenerater.GenerateId();
            RemoteAddress = (IPEndPoint)socket.RemoteEndPoint;
        }


        public abstract void Start();
        public abstract void Send(string message);

        protected void OnError(SocketError socketError)
        {
            mErrorCallBack?.Invoke(this, socketError);
        }

        private Action<BChannel, SocketError> mErrorCallBack;
        public event Action<BChannel, SocketError> OnErrorCallBack
        {
            add { mErrorCallBack += value; }
            remove { mErrorCallBack -= value; }
        }

        protected void OnReceived(byte[] message)
        {
            mReceiveCallBack?.Invoke(this, message);
        }

        private Action<BChannel, byte[]> mReceiveCallBack;
        public event Action<BChannel, byte[]> OnReceiveCallBack
        {
            add { mReceiveCallBack += value; }
            remove { mReceiveCallBack -= value; }
        }


        public long Id { get; private set; }

        public IPEndPoint RemoteAddress { get; set; }
    }
}
