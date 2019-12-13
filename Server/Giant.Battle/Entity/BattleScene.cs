using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public class BattleScene : Entity, IInitSystem<MapModel>, IUpdate
    {
        public MapModel MapModel { get; private set; }

        public void Init(MapModel model)
        {
            MapModel = model;

            AddComponentWithParent<UnitComponent>(this);
        }

        public void Update(double dt)
        { 
        }
    }
}
