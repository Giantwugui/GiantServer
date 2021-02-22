using Giant.Core;

namespace Giant.Battle
{
    public partial class Unit
    {
        protected TriggeComponent TriggeComponent { get; private set; }

        protected virtual void InitTrigger()
        {
            TriggeComponent = ComponentFactory.CreateComponent<TriggeComponent, Unit>(this);
        }
    }
}
