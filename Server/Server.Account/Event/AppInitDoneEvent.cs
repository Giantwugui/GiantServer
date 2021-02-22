using Giant.Core;
using Giant.EnumUtil;
using Giant.Net;

namespace Server.Account
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEvent : Event
    {
        public override void Handle()
        {
            Scene.Pool.AddComponent<GateInfoComponent>();
            Scene.Pool.AddComponent<HttpComponent, int>(Scene.AppConfig.HttpPort);
        }
    }
}
