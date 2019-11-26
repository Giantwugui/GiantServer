using Giant.Core;

namespace Giant.Framework
{
    public abstract class BaseManagerComponent : Component, IInitSystem<NetProxyComponent>
    {
        public NetProxyComponent NetProxyManager { get; private set; }

        public void Init(NetProxyComponent manager)
        {
            NetProxyManager = manager;
        }

        public abstract void NotifyServiceInfo(BackendComponent backend);
    }
}
