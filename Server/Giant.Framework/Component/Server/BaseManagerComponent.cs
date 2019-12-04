using Giant.Core;

namespace Giant.Framework
{
    public abstract class BaseManagerComponent : Component
    {
        public abstract void NotifyServiceInfo(BackendComponent backend);
    }
}
