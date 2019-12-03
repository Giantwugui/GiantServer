using Giant.Core;

namespace Giant.Framework
{
    public abstract class BaseManagerComponent : InitSystem
    {
        public override void Init() { }
        public abstract void NotifyServiceInfo(BackendComponent backend);
    }
}
