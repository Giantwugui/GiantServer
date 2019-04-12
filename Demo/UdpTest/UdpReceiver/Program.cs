using System;
using System.Threading;
using Giant.Net;

namespace UdpReceiver
{
    class Program
    {
        static NetworkService service;

        static void Main(string[] args)
        {
            try
            {
                service = new NetworkService(NetworkType.Tcp, "127.0.0.1", 8001);

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
                        service.Broadcase(content.ToUtf8Bytes());
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
                service.Update();
            }
        }
    }
}
