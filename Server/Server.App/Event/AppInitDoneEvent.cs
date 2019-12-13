using Giant.Core;
using Giant.Data;
using Giant.Framework;
using Giant.Net;
using System.Collections.Generic;

namespace Server.App
{
    [Event(EventType.InitDone)]
    public class AppInitDoneEvent : Event
    {
        public override void Handle()
        {
            switch (Scene.AppConfig.AppType)
            {
                case AppType.Global:
                    Scene.Pool.AddComponent<HttpBenchmarkComponent>();
                    Scene.Pool.AddComponent<HttpComponent, List<int>>(Scene.AppConfig.HttpPorts);
                    break;
                case AppType.Account:
                    Scene.Pool.AddComponent<GateInfoComponent>();
                    Scene.Pool.AddComponent<HttpComponent, List<int>>(Scene.AppConfig.HttpPorts);
                    break;
                case AppType.Gate:
                    Scene.Pool.AddComponent<BenchmarkComponent>();
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
