using Giant.Core;
using Giant.DB;
using System;

namespace Server.App
{
    public class Player : Field, IInitSystem<PlayerInfo>
    {
        private PlayerInfo playerInfo;

        public int Uid { get; private set; }
        public int MapId { get; private set; }
        public bool IsOnline { get; private set; }
        public DateTime OnlineTime { get; private set; }
        public DateTime OfflineTime { get; private set; }

        public void Init(PlayerInfo info)
        {
            playerInfo = info;
            Uid = info.Uid;
            MapId = info.MapId;

            IsOnline = true;
        }

        public void EnterWorld(int mapId)
        {
            //同步各种数据，任务，货币

            EnterMap(mapId);
        }

        public void EnterMap(int mapId)
        {
            MapComponent map = MapManangerComponent.Instance.GetMap(mapId);
        }

        public void Offline()
        {
            IsOnline = false;
            OfflineTime = TimeHelper.Now;

            PlayerManagerComponent.Instance.PlayerOffline(this);

            //TODO 数据落盘
        }

        public override void Dispose()
        {
        }
    }
}
