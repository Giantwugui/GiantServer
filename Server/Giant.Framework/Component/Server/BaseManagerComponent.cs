using Giant.Core;

namespace Giant.Framework
{
    public abstract class BaseManagerComponent : Component, IInitSystem
    {
        public void Init() { }

        public abstract void NotifyServiceInfo(BackendComponent backend);
    }
}
