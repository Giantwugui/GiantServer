using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class UnitComponent : Entity, IInitSystem<List<Unit>>
    {
        public void Init(List<Unit> units)
        {
            units.ForEach(x => AddChild(x));
        }

        public void AddUnit(Unit unit) => AddChild(unit);
        public Unit GetUnit(long instanceId) => GetChild<Unit>(instanceId);
        public void RemoveUnit(long instanceId) => RemoveChild(instanceId);
    }
}
