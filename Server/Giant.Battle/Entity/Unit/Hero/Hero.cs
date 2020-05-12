using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public partial class Hero : Unit, IInitSystem<MapScene, HeroModel>
    {
        public void Init(MapScene mapScene, HeroModel model)
        {
            base.Init(mapScene, UnitType.Hero);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override void UpdateInBattle(double dt)
        {
            base.UpdateInBattle(dt);
        }

        protected override bool IsAny(Unit unit)
        {
            return false;
        }

        protected override bool IsEnemy(Unit unit)
        {
            return false;
        }

        protected override bool IsAutoAI()
        {
            return false;
        }
    }
}
