namespace Giant.Battle
{
    public partial class Unit
    {
        protected NatureComponent NatureComponent { get; private set; }

        public void NatureChange(NatureType type, int value)
        {
            msgSource.OnNatureChange(this, type, value);
        }

        public void UpdateHP(int hp)
        {
            int value = NatureComponent.AddValue(NatureType.HP, hp);
            if (value <= 0)
            {
                IsDead = true;
                OnDead();
            }
        }
    }
}
