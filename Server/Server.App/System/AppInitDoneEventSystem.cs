using Giant.Core;

namespace Server.App
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEventSystem : Event
    {
        public override void Handle()
        {
            switch (Scene.AppConfig.AppType)
            {
                case AppType.Account:
                    Scene.Pool.AddComponent<GateInfoComponent>();
                    break;
                case AppType.Gate:
                    Scene.Pool.AddComponent<MapDataComponent>();
                    Scene.Pool.AddComponent<ClientManagerComponent>();
                    break;
                case AppType.Zone:
                    Scene.Pool.AddComponent<MapDataComponent>();
                    break;
                case AppType.Manager:
                    Scene.Pool.AddComponent<UidComponent>();
                    break;
            }
        }
    }
}
