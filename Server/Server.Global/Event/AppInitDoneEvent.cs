using Giant.Core;
using Giant.EnumUtil;
using Giant.Framework;
using Giant.Net;

namespace Server.Global
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEvent : Event
    {
        public override void Handle()
        {
            Scene.Pool.AddComponent<HttpBenchmarkComponent>();
            Scene.Pool.AddComponent<HttpComponent, int>(Scene.AppConfig.HttpPort);
        }
    }
}
