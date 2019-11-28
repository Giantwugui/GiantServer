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
                    Scene.Pool.AddComponentWithCreate<GateInfoComponent>();
                    break;
                case AppType.Gate:
                    Scene.Pool.AddComponentWithCreate<MapDataComponent>();
                    Scene.Pool.AddComponentWithCreate<ClientManagerComponent>();
                    break;
                case AppType.Zone:
                    Scene.Pool.AddComponentWithCreate<MapDataComponent>();
                    break;
                case AppType.Manager:
                    Scene.Pool.AddComponentWithCreate<UidComponent>();
                    break;
            }
        }
    }
}
