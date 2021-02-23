namespace Giant.Battle
{
    partial class Unit
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
            OnDamage(this, damage);
        }
    }
}
