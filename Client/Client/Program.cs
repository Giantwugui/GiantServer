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
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
                networkService = new OutterNetworkService(NetworkType.Tcp);
                networkService.MessageParser = new ProtoPacker();
                networkService.MessageDispatcher = new MessageDispatcher();

                session = networkService.Create(endPoint);
                session.Start();

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        private static void DoCmd(string content)
        {
            string[] param = content.Split(' ');
            if (param.Length <= 0)
            {
                Console.WriteLine("Param error !");
            }

            switch (param[0])
            {
                case "Login":
                    {
                        DoLogin(param[1]);
                    }
                    break;
                default:
                    Console.WriteLine("Not suport cmd !");
                    break;
            }
        }

        private static async void DoLogin(string account)
        {
            Msg_CG_Login login = new Msg_CG_Login
            {
                Account = account,
            };

            Msg_GC_Login result = await session.Call(login) as Msg_GC_Login;
            if (result.Error == ErrorCode.Success)
            {
                Console.WriteLine($"Client login success {login.Account}");
            }
        }

    }
}