using Giant.Net;
using Giant.Share;
using System;
using System.Reflection;

namespace Client
{
    public partial class NET
    {
        static NetworkService networkService;

        static Session session;
        public static Session Session => session;

        public static bool IsConnected => session != null && session.IsConnected;

        public static void Init()
        {
            networkService = new OutterNetworkService(NetworkType.Tcp)
            {
                MessageParser = new ProtoPacker(),
                MessageDispatcher = new MessageDispatcher()
            };
            RegistHandler();

            session = networkService.Create("127.0.0.1:8001");
            session.OnConnectCallback += OnConnect;
            session.Start();
        }

        public static void Update()
        {
        }

        private static void RegistHandler()
        {
            networkService.MessageDispatcher.RegisterHandler(AppType.AllServer, Assembly.GetEntryAssembly());
        }

        private static void OnConnect(Session session, bool state)
        {
            if (!state)
            {
                Console.WriteLine($"Disconnected {session.Id}");
            }
        }
    }
}
