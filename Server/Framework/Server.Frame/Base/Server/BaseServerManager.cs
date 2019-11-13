namespace Server.Frame
{
    public abstract class BaseServerManager
    {
        public NetProxyManager NetProxyManager { get; private set; }

        public BaseServerManager(NetProxyManager manager)
        {
            NetProxyManager = manager;
        }

        public abstract void NotifyServiceInfo(BackendServer backend);
    }
}
