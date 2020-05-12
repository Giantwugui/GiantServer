using UnityEngine;

namespace Giant.Battle
{
    partial class Unit : IBattleAction
    {
        public virtual void StartFighting() { }
        public virtual void StopFighting() { }

        public virtual void OnHit(Unit target, int damage)
        {

            //TODO 伤害值计算

            UpdateHP(-damage);
        }

        public void Damage(int damage)
        {
            MsgSource.OnDamage(this, damage);
        }
    }
}
