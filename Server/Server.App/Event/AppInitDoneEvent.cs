using Giant.Battle;
using Giant.Core;
using Giant.Framework;
using Giant.Net;
using System.Collections.Generic;
using System.Reflection;

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
                case AppType.Manager:
                    Scene.Pool.AddComponent<UidComponent>();
                    Scene.Pool.AddComponent<UpdateIndexComponent>();
                    break;
                case AppType.Gate:
                    Scene.Pool.AddComponent<BenchmarkComponent>();
                    Scene.Pool.AddComponent<MapDataComponent>();
                    Scene.Pool.AddComponent<ClientManagerComponent>();
                    break;
                case AppType.Zone:
                    //注册战斗回调事件
                    Scene.EventSystem.Regist(typeof(BattleScene).Assembly);

                    MapDataComponent mapData = Scene.Pool.AddComponent<MapDataComponent>();
                    Scene.Pool.AddComponent<MapGridPosComponent>();
                    Scene.Pool.AddComponent<MapManangerComponent, MapDataComponent>(mapData);
                    Scene.Pool.AddComponent<PlayerManagerComponent>();
                    Scene.Pool.AddComponent<BattleSceneComponent>();
                    break;
            }
        }
    }
}
