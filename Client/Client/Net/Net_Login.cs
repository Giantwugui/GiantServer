using Giant.Msg;
using Giant.Net;
using System;
using System.Linq;
using System.Net;

namespace Client
{
    public partial class NET
    {
        private static async void Login()
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

        private static async void GetGate()
        {
            Msg_CA_LoginZone msg = new Msg_CA_LoginZone()
            {
                Account = accountInfo.Account,
                Zone = 1,
            };

            Msg_AC_LoginZone result = (await session.Call(msg)) as Msg_AC_LoginZone;
            if (result.Error == ErrorCode.Success)
            {
                Console.WriteLine($"Client login to gate IP {result.IP} port {result.Port}");

                ConnectToGate(result.IP, result.Port);
            }
        }

        private static void ConnectToGate(string ip, int port)
        {
            session = networkService.Create(new IPEndPoint(IPAddress.Parse(ip), port));
            Session.OnConnectCallback += (aimSession, state) =>
            {
                if (state)
                {
                    GetCharacter();
                }
                else
                {
                    Console.WriteLine($"Session {aimSession.Id} disconnected from gate {aimSession.RemoteIPEndPoint.ToString()}");
                    session.Dispose();
                }
            };
            session.Start();
        }

        private static async void GetCharacter()
        {
            Msg_CG_GetCharacter msg = new Msg_CG_GetCharacter();
            Msg_GC_GetCharacter result = (await session.Call(msg) as Msg_GC_GetCharacter);
            SetCharacters(result.Characters);

            if (result.IsSuccess())
            {
                LoginWorld(result.Characters.First().Uid);
            }
            else
            {
                CreateCharacter(1);
            }
        }

        private static async void CreateCharacter(int roleId)
        {
            Msg_CG_CreateCharacter msg = new Msg_CG_CreateCharacter { RoleId = roleId };
            Msg_GC_CreateCharacter result = (await session.Call(msg)) as Msg_GC_CreateCharacter;
            if (result.IsSuccess())
            {
                LoginWorld(result.Character.Uid);
            }
        }

        private static async void LoginWorld(int uid)
        {
            Msg_CG_Login msg = new Msg_CG_Login() { Uid = uid };
            Msg_GC_Login result = (await session.Call(msg)) as Msg_GC_Login;
            if (result.IsSuccess())
            {
                Player = new Player(accountInfo.Account, result.PlayerInfo, session);
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
