using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;

namespace Giant.Battle
{
    public partial class MapScene : Entity, IInitSystem<int, int>, IUnitContainer
    {
        private int unitId = 0;

        public double StartTime { get; private set; }
        public int MapId { get; private set; }
        public int Channel { get; private set; }
        public MapModel MapModel { get; private set; }

        public AOIType AOIType => MapModel.AOIType;

        public virtual void Init(int mapId, int channel)
        {
            MapId = mapId;
            Channel = channel;
            MapModel = MapLibrary.Instance.GetModel(mapId);

            InitNPC();
            InitRegionManager();
        }

        public virtual void Update(double dt)
        {
            StartTime += dt;

            UpdateNpc(dt);
            UpdateHero(dt);
            UpdatePlayer(dt);
            UpdateMonster(dt);
        }

        public int GetUnitId()
        {
            return ++unitId;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void InitNPC()
        { 
        }
    }
}
