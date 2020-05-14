using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public partial class Hero : Unit, IInitSystem<MapScene, UnitInfo, HeroModel>
    {
        private UnitInfo unitInfo;
        
        public void Init(MapScene mapScene, UnitInfo info, HeroModel model)
        {
            base.Init(mapScene, UnitType.Hero);

            unitInfo = info;
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override void InitNature()
        {
            NatureComponent.Add(unitInfo?.Natures);
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
