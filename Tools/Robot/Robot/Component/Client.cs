using Giant.Core;
using Giant.Msg;
using Giant.Net;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Robot
{
    public class Client : InitSystem<string>
    {
        private int token;
        private string publicKey;
        private Session session;
        private AccountInfo accountInfo;
        private string accountHost = "127.0.0.1:6002";
        private List<int> loginedServers = new List<int>();
        private Dictionary<int, Msg_CharacterInfo> characterInfos = new Dictionary<int, Msg_CharacterInfo>();

        public string Account { get; private set; }
        public bool IsEnterWorlded { get; private set; }


        public override void Init(string account)
        {
            Account = account;

            string path = Directory.GetCurrentDirectory();
            string publicKeyPath = Path.Combine(path, "PrivateKey.txt");
            if (File.Exists(publicKeyPath))
            {
                publicKey = File.ReadAllText(publicKeyPath);
            }
        }

        private void SetCharacters(IEnumerable<Msg_CharacterInfo> characters)
        {
            characterInfos.Clear();
            foreach (var kv in characters)
            {
                characterInfos.Add(kv.Uid, kv);
            }
        }

        private void SetLoginedServers(IEnumerable<int> servers)
        {
            loginedServers.Clear();
            loginedServers.AddRange(servers);
        }

        public void DoLogin()
        {
            accountInfo = new AccountInfo(Account, "");
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

        private async void Login()
        {
            Msg_CA_Login msg = new Msg_CA_Login()
            {
                Account = accountInfo.Account,
                Password = accountInfo.Passward,
            };

            var result = await session.Call(msg);
            Msg_AC_Login logResult = result as Msg_AC_Login;
            if (result.IsSuccess())
            {
                Console.WriteLine($"user login success {accountInfo.Account}, logined servers {string.Join("|", logResult.Servers)}");

                SetLoginedServers(logResult.Servers);
                GetGate();
            }
        }

        private async void GetGate()
        {
            Msg_CA_LoginZone msg = new Msg_CA_LoginZone()
            {
                Account = accountInfo.Account,
                Zone = 1001,
            };

            Msg_AC_LoginZone result = (await session.Call(msg)) as Msg_AC_LoginZone;
            if (result.Error == ErrorCode.Success)
            {
                Console.WriteLine($"Client login to gate address {result.Address}");

                token = result.Token;
                ConnectToGate(result.Address);
            }
        }

        private void ConnectToGate(string address)
        {
            session?.Dispose();
            session = Scene.Pool.GetComponent<OutterNetworkComponent>().Create(address);
            session.OnConnectCallback += (aimSession, state) =>
            {
                if (state)
                {
                    GetEncyptKey();

                    session.OnConnectCallback += (session, state) =>
                    {
                        var player = PlayerManagerComponent.Instance.GetPlayer(session.InstanceId);
                        player?.OnDisconnected();
                    };
                }
                else
                {
                    Console.WriteLine($"Session {aimSession.Id} disconnected from gate {aimSession.RemoteIPEndPoint.ToString()}");
                    session.Dispose();
                }
            };
            session.Start();
        }

        private async void GetEncyptKey()
        {
            Msg_CG_Get_SecretKey msg = new Msg_CG_Get_SecretKey();
            Msg_GC_Get_SecretKey result = (await session.Call(msg)) as Msg_GC_Get_SecretKey;

            byte[] aesBytes = RSAHelper.Decrypt(result.SecretKey.FromBase64String(), publicKey);

            session.IsNeedEncrypt = true;
            session.AESCrypt.SetCryptKey(aesBytes.ToBase64String());

            GetCharacter();
        }

        private async void GetCharacter()
        {
            Msg_CG_GetCharacter msg = new Msg_CG_GetCharacter();
            Msg_GC_GetCharacter result = (await session.Call(msg) as Msg_GC_GetCharacter);
            SetCharacters(result.Characters);

            if (result.IsSuccess())
            {
                if (result.Characters.Count > 0)
                {
                    EnterWorld(result.Characters.First().Uid);
                }
                else
                {
                    CreateCharacter(1);
                }
            }
        }

        private async void CreateCharacter(int roleId)
        {
            Msg_CG_CreateCharacter msg = new Msg_CG_CreateCharacter { RoleId = roleId };
            Msg_GC_CreateCharacter result = (await session.Call(msg)) as Msg_GC_CreateCharacter;
            if (result.IsSuccess())
            {
                EnterWorld(result.Character.Uid);
            }
        }

        private void EnterWorld(int uid)
        {
            IsEnterWorlded = true;
            Player player = ComponentFactory.CreateComponent<Player, string, int, Session>(accountInfo.Account, uid, session);
            PlayerManagerComponent.Instance.AddPlayer(player);
            player.EnterWorld();

            int count = PlayerManagerComponent.Instance.PlayerCount();
            if (count % 20 == 0)
            {
                Console.WriteLine($"total count {ClientManagerComponent.Instance.TotalCount} login {count} cost {ClientManagerComponent.Instance.Stopwatch.ElapsedMilliseconds}");
            }
        }
    }
}
