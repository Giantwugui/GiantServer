using Giant.Core;
using Giant.Model;

namespace Server.App
{
    public class DataLabComponent : InitSystem<AppType>
    {
        public override void Init(AppType appType)
        {
            switch (appType)
            {
                case AppType.Zone:
                    Scene.Pool.AddComponent<MapLibrary>();
                    Scene.Pool.AddComponent<DungeonLibrary>();
                    Scene.Pool.AddComponent<MonsterLibrary>();
                    Scene.Pool.AddComponent<SkillLibrary>();
                    Scene.Pool.AddComponent<BuffLibrary>();
                    break;
                default:
                    break;
            }
        }
    }
}
