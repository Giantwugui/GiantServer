using Giant.Core;

namespace Giant.Battle
{
    public class BattleSceneComponent : Entity, IInitSystem, IUpdateSystem
    {
        public void Update(double dt)
        {
        }

        public void AddBattleScene(Unit unit) => AddChild(unit);
        public Unit GetBattleScene(long instanceId) => GetChild<Unit>(instanceId);
        public void GetBattleUnit(long instanceId) => RemoveChild(instanceId);
    }
}
