using Giant.Core;
using Giant.Framework;

namespace Server.Gate
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEvent : Event
    {
        public override void Handle()
        {
            Scene.Pool.AddComponent<BenchmarkComponent>();
            Scene.Pool.AddComponent<ClientManagerComponent>();
        }
    }
}
