namespace Server.Frame
{
    public abstract class BaseServiceManager
    {
        public NetProxyManager NetProxyManager { get; private set; }

        public BaseServiceManager(NetProxyManager manager)
        {
            NetProxyManager = manager;
        }

        public abstract void NotifyServiceInfo(BackendService backend);
    }
}
