using Giant.Core;

namespace Giant.Battle
{
    public static class BattleSceneFactory
    {
        public static BattleScene CreateBattleScene(MapModel model)
        {
            switch (model.MapType)
            {
                case MapType.Normal:
                default:
                    return ComponentFactory.CreateComponentWithParent<BattleScene>(BattleSceneComponent.Instance);
            }
        }
    }
}
