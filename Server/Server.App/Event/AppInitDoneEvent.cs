using Giant.Core;
using Giant.Framework;

namespace Server.App
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEvent : Event
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
                    MapDataComponent mapData = Scene.Pool.AddComponent<MapDataComponent>();
                    Scene.Pool.AddComponent<MapGridPosComponent>();
                    Scene.Pool.AddComponent<MapManangerComponent, MapDataComponent>(mapData);
                    Scene.Pool.AddComponent<PlayerManagerComponent>();
                    break;
                case AppType.Manager:
                    Scene.Pool.AddComponent<UidComponent>();
                    Scene.Pool.AddComponent<UpdateIndexComponent>();
                    break;
            }
        }
    }
}
