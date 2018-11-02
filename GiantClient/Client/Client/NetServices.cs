using GiantCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class NetServices
    {
        public static void ToStart()
        {
            mClientSocket = new CSocket("127.0.0.1", 8099);

            mClientSocket.OnConnected += OnConnected;
            mClientSocket.OnReceiveMessage += OnReceive;

            mClientSocket.ToStart();
        }

        public static void Send(OuterMessage message)
        {
            mClientSocket.Send(message.ToProtoBytes());
        }

        public static void Send(byte[] content)
        {
            mClientSocket.Send(content);
        }

        private static void OnConnected(bool conn)
        {
            Console.WriteLine(conn ? "success" : "fail");
        }


        private static void OnReceive(byte[] message)
        {
            Console.WriteLine(Encoding.UTF8.GetString(message));
        }


        static CSocket mClientSocket = null;

    }
}
