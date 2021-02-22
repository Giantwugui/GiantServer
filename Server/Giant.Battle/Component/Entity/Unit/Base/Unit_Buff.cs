namespace Giant.Battle
{
    public partial class Unit
    {
        protected BuffComponent BuffComponent { get; private set; }

        protected virtual void InitBuff()
        {
            BuffComponent = AddComponentWithParent<BuffComponent>();
        }

        public void AddBuff(int buffId)
        {
            BuffComponent.AddBuff(buffId);
        }

        public void RemoveBuff(int buffId)
        {
            BuffComponent.RemoveBuff(buffId);
        }
    }
}
