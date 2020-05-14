using Giant.Data;
using UnityEngine;

namespace Giant.Battle
{
    partial class Unit : IBattleAction
    {
        public int HateRate { get; private set; }

        public virtual void StartFighting() { }
        public virtual void StopFighting() { }

        public virtual void OnHit(Unit target, int damage)
        {
            //TODO 伤害值计算

            target?.HateComponent.AddHate(Id, HateRate);

            UpdateHP(-damage);
        }

        public void Damage(int damage)
        {
            MsgSource.OnDamage(this, damage);
        }
    }
}
