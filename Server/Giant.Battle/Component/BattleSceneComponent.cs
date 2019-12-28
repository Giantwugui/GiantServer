using Giant.Core;

namespace Giant.Battle
{
    public class BattleSceneComponent : SingleEntity<BattleSceneComponent>, IUpdateSystem
    {
        public void Update(double dt)
        {
        }

        public void AddBattleScene(Unit unit) => AddChild(unit);
        public Unit GetBattleScene(long instanceId) => GetChild<Unit>(instanceId);
        public void RemoveBattleScene(long instanceId) => RemoveChild(instanceId);
    }
}
