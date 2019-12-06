using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Robort
{
    public partial class Player
    {
        private AccountInfo accountInfo;
        private string accountHost = "127.0.0.1:6002";

        private int token;
        private List<int> loginedServers = new List<int>();
        private Dictionary<int, Msg_CharacterInfo> characterInfos = new Dictionary<int, Msg_CharacterInfo>();

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
                    LoginZone();
                    session.OnConnectCallback += PlayerOffline;

                    PlayerManagerComponent.Instance.AddPlayer(this);
                }
                else
                {
                    Console.WriteLine($"Session {aimSession.Id} disconnected from gate {aimSession.RemoteIPEndPoint.ToString()}");
                    session.Dispose();
                }
            };
            session.Start();
        }

        private async void LoginZone()
        {
            Msg_CG_Login msg = new Msg_CG_Login() { Account = accountInfo.Account, Token = token };
            Msg_GC_Login result = (await session.Call(msg)) as Msg_GC_Login;
            if (result.IsSuccess())
            {
                GetCharacter();
            }
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
            PlayerManagerComponent.Instance.EnterWorld(this);
            int count = PlayerManagerComponent.Instance.PlayerCount();
            if (count % 20 == 0)
            {
                Console.WriteLine($"total count {PlayerManagerComponent.Instance.TotalCount} login {count} cost {PlayerManagerComponent.Instance.Stopwatch.ElapsedMilliseconds}");
            }

            Msg_CG_EnterWorld msg = new Msg_CG_EnterWorld() { Uid = uid };
            session.Notify(msg);
        }

        private void PlayerOffline(Session session, bool state)
        {
            var player = PlayerManagerComponent.Instance.GetPlayer(session);
            player?.OnDisconnected();
        }
    }
}