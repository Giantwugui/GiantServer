using Giant.Core;

namespace Giant.Battle
{
    public class BattleScene : Entity, IInitSystem<MapModel>, IUpdate
    {
        public void Init(MapModel model)
        {
            AddComponentWithParent<MapComponent, MapModel>(model);
            AddComponentWithParent<UnitComponent>();
        }

        public void Update(double dt)
        {
            GetComponent<UnitComponent>().Update(dt);
        }
    }
}
