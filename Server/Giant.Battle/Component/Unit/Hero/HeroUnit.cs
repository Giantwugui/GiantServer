using Giant.Core;
using Giant.Model;

namespace Giant.Battle
{
    public partial class HeroUnit : Unit, IInitSystem<MapScene, UnitInfo, HeroModel>
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
            switch (unit.UnitType)
            {
                case UnitType.Player:
                case UnitType.Hero:
                    return true;
                case UnitType.Monster:
                    return false;
            }
            return false;
        }

        protected override bool IsEnemy(Unit unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                case UnitType.Hero:
                    return false;
                case UnitType.Monster:
                    return true;
            }
            return false;
        }

        protected override bool IsAutoAI()
        {
            return false;
        }
    }
}
