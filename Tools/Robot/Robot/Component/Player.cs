using Giant.Core;
using Giant.Msg;
using Giant.Net;

namespace Robot
{
    public partial class Player : InitSystem<string, int, Session>
    {
        private Msg_PlayerInfo playerInfo;
        private long heartBeatLastTime = TimeHelper.NowSeconds;

        private Session session;
        public Session Session => session;
        public string Account { get; private set; }
        public int Uid { get; private set; }

        public override void Init(string account, int uid, Session session)
        {
            Account = account;
            Uid = uid;
            this.session = session;

            AddComponent<>
        }

        public void SetPlayerInfo(Msg_PlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
        }

        public void Update()
        {
            CheckHeartBeat();
        }

        public void EnterWorld()
        {
            Msg_CG_EnterWorld msg = new Msg_CG_EnterWorld() { Uid = Uid };
            session.Notify(msg);
        }

        public void OnDisconnected()
        {
            //TODO 短线重连逻辑
            Dispose();
        }

        public override void Dispose()
        {
            PlayerManagerComponent.Instance.RemovePlayer(this);

            Dispose();
            session.Dispose();
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
