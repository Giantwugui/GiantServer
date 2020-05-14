using Giant.Battle;
using Giant.Core;
using Giant.Data;

namespace Server.App
{
    public class DataLabComponent : InitSystem<AppType>
    {
        public override void Init(AppType appType)
        {
            switch (appType)
            {
                case AppType.Zone:
                    Scene.Pool.AddComponent<MapLibComponent>();
                    Scene.Pool.AddComponent<DungeonLibComponent>();
                    Scene.Pool.AddComponent<MonsterLibComponent>();
                    Scene.Pool.AddComponent<SkillLibComponent>();
                    Scene.Pool.AddComponent<BuffLibComponent>();
                    break;
                default:
                    break;
            }
        }
    }
}
