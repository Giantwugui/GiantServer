using System;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class Client
    {
        protected Socket Socket { get; private set; }

        public IPEndPoint ipEndPoint { get; private set; }


        public Client(Socket socket)
        {
        }

        public Client(string host, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
        }

    }
}
