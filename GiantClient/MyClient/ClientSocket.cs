using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyClient
{
    class ClientSocket
    {

        public ClientSocket(string ip, int port)
        {
            mEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            mSocket = new Socket(AddressFamily.InterNetwork | AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            ToConn();
        }

        private void ToConn()
        {
            mSocket.BeginConnect(mEndPoint, AsyncConnect, null);
        }

        private void AsyncConnect(IAsyncResult ar)
        {
            try
            {
                mSocket.EndConnect(ar);
            }
            catch
            {
            }
        }

        public Action<bool> OnClosed;

        public Action<bool> OnConnected;

        public Action<byte[]> OnReceivedMessage;

        public bool IsConnected { get { return mSocket.Connected; } }

        public int Port { get { return mEndPoint.Port; } }

        public IPAddress IP { get { return mEndPoint.Address; } }

        private readonly Socket mSocket = null;

        private readonly IPEndPoint mEndPoint = null;
    }
}
