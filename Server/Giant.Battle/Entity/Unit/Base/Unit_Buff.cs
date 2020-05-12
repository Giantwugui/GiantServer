namespace Giant.Battle
{
    public partial class Unit
    {
        protected BuffComponent BuffComponent { get; private set; }

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
