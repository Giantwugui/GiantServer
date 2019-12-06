using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System;
using System.Collections.Generic;

namespace Robort
{
    public partial class Player : InitSystem<string>
    {
        private Msg_PlayerInfo playerInfo;
        private long heartBeatLastTime = TimeHelper.NowSeconds;

        public string Account { get; private set; }
        public int Uid { get; private set; }

        public override void Init(string account)
        {
            this.Account = account;
        }

        public void SetPlayerInfo(Msg_PlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
        }

        public void Update()
        {
            CheckHeartBeat();
        }



        private Session session;
        public Session Session => session;

        private void SetCharacters(IEnumerable<Msg_CharacterInfo> characters)
        {
            characterInfos.Clear();
            foreach (var kv in characters)
            {
                characterInfos.Add(kv.Uid, kv);
            }
        }

        public void OnDisconnected()
        {
            //TODO 短线重连逻辑
            this.Dispose();
        }

        public override void Dispose()
        {
            PlayerManagerComponent.Instance.RemovePlayer(this);
            
            Dispose();
            session.Dispose();
        }

        private void SetLoginedServers(IEnumerable<int> servers)
        {
            loginedServers.Clear();
            loginedServers.AddRange(servers);
        }

        private async void CheckHeartBeat()
        {
            if (Session == null || !Session.IsConnected)
            {
                return;
            }

            if ((TimeHelper.NowSeconds - heartBeatLastTime) > 30)
            {
                heartBeatLastTime = TimeHelper.NowSeconds;

                Msg_CG_HeartBeat_Ping msg = new Msg_CG_HeartBeat_Ping();
                await Session.Call(msg);
            }
        }
    }
}
