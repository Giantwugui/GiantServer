using Giant.Core;
using Giant.Data;
using UnityEngine;

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
