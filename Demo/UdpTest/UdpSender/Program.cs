using System;
using System.Net;
using System.Threading;
using Giant.Net;

namespace UdpReceiver
{
    class Program
    {
        static NetworkService networkService;
        static Session session;

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
                networkService = new NetworkService(NetworkType.Tcp);

                session = networkService.CreateSession(endPoint);


                Thread thread = new Thread(Loop);
                thread.Name = "Update";
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            while (true)
            {
                try
                { 
                    Thread.Sleep(1);

                    string content = Console.ReadLine();
                    if (!string.IsNullOrEmpty(content))
                    {
                        session.Transfer(content.ToUtf8Bytes());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void Loop()
        {
            while (true)
            {
                Thread.Sleep(1);
                networkService.Update();
            }
        }
    }
}