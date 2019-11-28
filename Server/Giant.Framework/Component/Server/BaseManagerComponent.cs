using Giant.Core;

namespace Giant.Framework
{
    public abstract class BaseManagerComponent : Component, IInitSystem
    {
        public abstract void NotifyServiceInfo(BackendComponent backend);
    }
}
