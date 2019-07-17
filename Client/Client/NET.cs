using Giant.Net;
using Giant.Share;
using Giant.Msg;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Client
{
    class AccountInfo
    {
        public string Account { get; private set; }
        public string Passward { get;  private set; }

        public AccountInfo(string account, string passward)
        {
            this.Account = account;
            this.Passward = passward;
        }
    }

    public partial class NET
    {
        static NetworkService networkService;
        private static readonly Dictionary<long, AccountInfo> waittingLoginList = new Dictionary<long, AccountInfo>();

        public static void Init()
        {
            networkService = new OutterNetworkService(NetworkType.Tcp)
            {
                MessageParser = new ProtoPacker(),
                MessageDispatcher = new MessageDispatcher()
            };
            RegistHandler();
        }

        public static void DoLogin(string account, string passward)
        {
            Session session = networkService.Create("127.0.0.1:8001");
            AddToWatting(session.Id, new AccountInfo(account, passward));

            session.OnConnectCallback += (aimSession, state)=>
            {
                if (state)
                {
                    Login(aimSession);
                }
                else
                {
                    Console.WriteLine($"Session {aimSession.Id} disconnected from {aimSession.RemoteIPEndPoint.ToString()}");

                    PlayerOffline(aimSession);
                }
            };

            session.Start();
        }

        private static void RegistHandler()
        {
            networkService.MessageDispatcher.RegisterHandler(AppType.AllServer, Assembly.GetEntryAssembly());
        }

        private static void AddToWatting(long sessionId, AccountInfo info)
        {
            waittingLoginList.Add(sessionId, info);
        }

        private static AccountInfo GetWattingInfo(long sessionId)
        {
            waittingLoginList.TryGetValue(sessionId, out var info);
            return info;
        }

        private static async void Login(Session session)
        {
            var accountInfo = GetWattingInfo(session.Id);
            if (accountInfo == null)
            {
                return;
            }

            Msg_CG_Login msg = new Msg_CG_Login()
            {
                Account = accountInfo.Account,
            };

            Msg_GC_Login result = await session.Call(msg) as Msg_GC_Login;
            if (result.Error == ErrorCode.Success)
            {
                Console.WriteLine($"Client login success {accountInfo.Account}");

                Player player = new Player(accountInfo.Account, 1001, session);
                PlayerManager.Instance.AddPlayer(player);
            }
        }

        private static void PlayerOffline(Session session)
        {
            var player = PlayerManager.Instance.GetPlayer(session);
            player?.OnDisconnected();
            session.Dispose();
        }
             
    }
}
