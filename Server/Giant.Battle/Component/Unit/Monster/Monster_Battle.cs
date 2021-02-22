using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Monster
    {
        public override void StartFighting()
        {
            base.StartFighting();
        }

        public override void StopFighting()
        {
            base.StopFighting();
        }

        protected override void UpdateInBattle(double dt)
        {
            base.UpdateInBattle(dt);
        }

        public override void OnDead()
        {
            base.OnDead();
        }

        public override void OnHit(Unit target, int damage)
        {
            base.OnHit(target, damage);
        }
    }
}
