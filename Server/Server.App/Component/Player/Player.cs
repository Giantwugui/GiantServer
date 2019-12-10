using Giant.Core;
using Giant.DB;

namespace Server.App
{
    public class Player : InitSystem<PlayerInfo>
    {
        private PlayerInfo playerInfo;

        public int MapId { get; private set; }

        public override void Init(PlayerInfo info)
        {
            playerInfo = info;
            MapId = info.MapId;
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
        }
    }
}
