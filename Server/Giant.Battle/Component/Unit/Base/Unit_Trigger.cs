using Giant.Core;

namespace Giant.Battle
{
    public partial class Unit
    {
        protected TriggerComponent TriggeComponent { get; private set; }

        protected virtual void InitTrigger()
        {
            TriggeComponent = ComponentFactory.Create<TriggerComponent, Unit>(this);
        }
    }
}
