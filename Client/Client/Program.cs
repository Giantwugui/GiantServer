using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System;
using System.Net;
using System.Threading;

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
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9091);
                networkService = new NetworkService(NetworkType.Tcp);

                session = networkService.Create(endPoint);
                networkService.MessageDispatcher = new MessageDispatcher();

                ConsoleReader.Instance.Start(DoCmd);
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
                    networkService.Update();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        private static void DoCmd(string content)
        {
            switch (content)
            {
                case "Login":
                    {
                        DoLogin();
                    }
                    break;
                default:
                    Console.WriteLine("Not suport cmd !");
                    break;
            }
        }

        private static async void DoLogin()
        {
            CR_LOGIN login = new CR_LOGIN
            {
                Account = "1000001",
                Password = "000000",
            };

            RC_LOGIN result = await session.Call(login) as RC_LOGIN;
            if (result.Error == ErrorCode.ERR_Success)
            {
                Console.WriteLine($"Client login success {login.Account}");
            }
        }

    }
}