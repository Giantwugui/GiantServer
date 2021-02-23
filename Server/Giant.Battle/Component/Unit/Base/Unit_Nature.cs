using Giant.EnumUtil;

namespace Giant.Battle
{
    public partial class Unit
    {
        protected NatureComponent NatureComponent { get; private set; }

        protected virtual void InitNature() 
        {
            NatureComponent = AddComponentWithParent<NatureComponent>();
        }

        public void NatureChange(NatureType type, int value)
        {
            OnNatureChange(type, value);
        }

        public void UpdateHP(int hp)
        {
            int value = NatureComponent.AddValue(NatureType.HP, hp);
            if (value <= 0)
            {
                IsDead = true;
                Dead();
            }
        }

        public int GetNatureValue(NatureType natureType)
        {
            return NatureComponent.GetNatureValue(natureType);
        }
    }
}
