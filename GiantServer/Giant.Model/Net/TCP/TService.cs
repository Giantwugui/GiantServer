using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Giant.Model
{
    /// <summary>
    /// 用于侦听客户端连接请求(只处理连接请求)
    /// </summary>
    class TService : BaseService
    {
        public TService(IPEndPoint endPoint, Action<BChannel> acceptCallback)
        {
            mAccepter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mAccepter.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            mAccepter.Bind(endPoint);
            mAccepter.Listen(1000);

            AcceptCallBack = acceptCallback;

            mAccepter.AcceptAsync();
        }

        private void AcceptLoop()
        {
            while (true)
            {
                try
                {
                    Socket socket = mAccepter.Accept();

                    TChannel channel = new TChannel(socket);

                    AcceptCallBack(channel);
                }
                catch
                {
                }
                Thread.Sleep(0);
            }
        }

        private Socket mAccepter;
    }
}
