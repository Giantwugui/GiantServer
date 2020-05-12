namespace Giant.Battle
{
    public partial class Unit
    {
        protected FsmComponent FsmComponent { get; private set; }

        protected virtual void InitFsm() 
        {
            FsmComponent = AddComponentWithParent<FsmComponent>(this);
        }
    }
}
