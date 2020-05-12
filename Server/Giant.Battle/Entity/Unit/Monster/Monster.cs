using Giant.Core;
using Giant.Data;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Monster : Unit, IInitSystem<MapScene, MonsterModel>
    {
        public void Init(MapScene mapScene, MonsterModel model)
        {
            base.Init(mapScene, UnitType.Monster);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
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
