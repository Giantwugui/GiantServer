using Giant.Core;

namespace Giant.Battle
{
    public partial class MapScene : Entity, IInitSystem<MapModel>, IUpdate
    {
        private int unitId = 0;

        public double StartTime { get; private set; }
        public MapComponent MapComponent { get; private set; }
        public MapModel MapModel=>MapComponent.Model;

        public virtual void Init(MapModel model)
        {
            MapComponent = ComponentFactory.CreateComponentWithParent<MapComponent, MapModel>(this, model);
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
    }
}
