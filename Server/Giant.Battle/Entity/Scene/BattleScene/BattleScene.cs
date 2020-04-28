using Giant.Core;

namespace Giant.Battle
{
    public partial class BattleScene : MapScene, IInitSystem<MapModel>, IUpdate
    {
        public override void Init(MapModel model) 
        {
            base.Init(model);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
