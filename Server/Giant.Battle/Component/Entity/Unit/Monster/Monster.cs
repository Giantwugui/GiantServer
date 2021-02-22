using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;

namespace Giant.Battle
{
    public partial class Monster : Unit, IInitSystem<MapScene, MonsterModel>
    {
        public MonsterModel MonsterModel { get; protected set; }

        public void Init(MapScene mapScene, MonsterModel model)
        {
            base.Init(mapScene, UnitType.Monster);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override void InitNature()
        {
            NatureComponent.SetValue(NatureType.MaxHP, MonsterModel.MaxHP);
            NatureComponent.SetValue(NatureType.Attack, MonsterModel.Attack);
            NatureComponent.SetValue(NatureType.Defence, MonsterModel.Defense);
            NatureComponent.SetValue(NatureType.HP, NatureComponent.GetNatureValue(NatureType.MaxHP));
        }

        protected override bool IsAny(Unit unit)
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

        protected override bool IsEnemy(Unit unit)
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

        protected override bool IsAutoAI()
        {
            return false;
        }
    }
}
