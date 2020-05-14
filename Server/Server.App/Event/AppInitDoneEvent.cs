using Giant.Battle;
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
                case AppType.Manager:
                    Scene.Pool.AddComponent<UidComponent>();
                    Scene.Pool.AddComponent<UpdateIndexComponent>();
                    break;
                case AppType.Gate:
                    Scene.Pool.AddComponent<BenchmarkComponent>();
                    Scene.Pool.AddComponent<ClientManagerComponent>();
                    break;
                case AppType.Zone:
                    //注册战斗回调事件
                    Scene.EventSystem.Regist(typeof(BattleScene).Assembly);

                    //各种读表
                    Scene.Pool.AddComponent<DataLabComponent, AppType>(AppType.Zone);

                    //世界地图信息
                    Scene.Pool.AddComponent<MapGridPosComponent>();
                    Scene.Pool.AddComponent<MapManangerComponent, MapLibComponent>(Scene.Pool.GetComponent<MapLibComponent>());

                    Scene.Pool.AddComponent<PlayerManagerComponent>();
                    Scene.Pool.AddComponent<BattleSceneComponent>();
                    Scene.Pool.AddComponent<ScriptComponent>();
                    break;
            }
        }
    }
}
