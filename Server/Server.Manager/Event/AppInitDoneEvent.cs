using Giant.Core;
using Giant.Framework;

namespace Server.Manager
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEvent : Event
    {
        public override void Handle()
        {
            Scene.Pool.AddComponent<UidComponent>();
            Scene.Pool.AddComponent<UpdateIndexComponent>();
        }
    }
}
