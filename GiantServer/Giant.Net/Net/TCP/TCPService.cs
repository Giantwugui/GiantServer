using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Giant.Net
{
    class TCPService : BaseService
    {
        public TCPService(IPEndPoint endPoint, Action<TCPChannel> acceptCallback)
        {
            mAccepter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mAccepter.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            mAccepter.Bind(endPoint);
            mAccepter.Listen(100);

            mAcceptCallBack = acceptCallback;

            mAcceptThread = new Thread(AcceptLoop);
            mAcceptThread.IsBackground = true;
            mAcceptThread.Name = "AcceptLoop";
            mAcceptThread.Start();
        }

        private void AcceptLoop()
        {
            while (true)
            {
                try
                {
                    Socket socket = mAccepter.Accept();

                    TCPChannel channel = new TCPChannel(socket);

                    mAcceptCallBack(channel);
                }
                catch
                {
                }
                Thread.Sleep(0);
            }
        }

        private Thread mAcceptThread;

        private Socket mAccepter;

        private Action<TCPChannel> mAcceptCallBack;
    }
}
