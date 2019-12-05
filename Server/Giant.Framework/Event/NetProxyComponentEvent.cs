using Giant.Core;

namespace Giant.Framework
{
    [Event(EventType.InitDone)]
    public class NetProxyComponentEvent : Event
    {
        public override void Handle()
        {
            Scene.Pool.GetComponent<NetProxyComponent>().Start();
        }
    }
}
