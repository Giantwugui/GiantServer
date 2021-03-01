using Giant.Battle;
using Giant.Core;
using Giant.EnumUtil;
using Giant.Framework;
using Giant.Model;
using Giant.Net;

namespace Server.Zone
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
                    Scene.Pool.AddComponent<HttpComponent, int>(Scene.AppConfig.HttpPort);
                    break;
                case AppType.Zone:
                    //注册战斗回调事件
                    Scene.EventSystem.Regist(typeof(BattleScene).Assembly);

                    //各种读表
                    Scene.Pool.AddComponent<MapGridLibrary>();
                    Scene.Pool.AddComponent<MapLibrary>();
                    Scene.Pool.AddComponent<DungeonLibrary>();
                    Scene.Pool.AddComponent<MonsterLibrary>();
                    Scene.Pool.AddComponent<SkillLibrary>();
                    Scene.Pool.AddComponent<BuffLibrary>();
                    Scene.Pool.AddComponent<TriggerLibrary>();

                    //世界地图信息
                    Scene.Pool.AddComponent<MapSceneManangerComponent>();

                    Scene.Pool.AddComponent<PlayerManagerComponent>();
                    Scene.Pool.AddComponent<BattleSceneComponent>();
                    Scene.Pool.AddComponent<ScriptComponent>();
                    break;
            }
        }
    }
}
