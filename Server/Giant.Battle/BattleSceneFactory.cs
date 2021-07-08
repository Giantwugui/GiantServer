using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;

namespace Giant.Battle
{
    public static class BattleSceneFactory
    {
        public static BattleScene CreateBattleScene(MapModel model, int mapId, int channel)
        {
            switch (model.MapType)
            {
                case MapType.Normal:
                default:
                    return ComponentFactory.Create<BattleScene, int, int>(mapId, channel);
            }
        }
    }
}
