using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using Giant.Framework;

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
        static AccountInfo accountInfo;

        private static Session session;
        public static Session Session => session;

        private static string accountHost = "127.0.0.1:6002";

        private static int token;
        private static Player Player;
        private static List<int> loginedServers = new List<int>();
        private static Dictionary<int, Msg_CharacterInfo> characterInfos = new Dictionary<int, Msg_CharacterInfo>();

        public static void DoLogin(string account, string passward)
        {
            accountInfo = new AccountInfo(account, passward);

            session = Scene.Pool.GetComponent<OutterNetworkComponent>().Create(accountHost);
            session.OnConnectCallback += (aimSession, state) =>
            {
                if (state)
                {
                    Login();
                }
                else
                {
                    Console.WriteLine($"Session {aimSession.Id} disconnected from {aimSession.RemoteIPEndPoint.ToString()}");
                    session.Dispose();
                }
            };

            session.Start();
        }

        private static void SetLoginedServers(IEnumerable<int> servers)
        {
            loginedServers.Clear();
            loginedServers.AddRange(servers);
        }

        private static void SetCharacters(IEnumerable<Msg_CharacterInfo> characters)
        {
            characterInfos.Clear();
            foreach (var kv in characters)
            {
                characterInfos.Add(kv.Uid, kv);
            }
        }

        private static Msg_CharacterInfo GetCharacter(int uid)
        {
            characterInfos.TryGetValue(uid, out var info);
            return info;
        }
    }
}
