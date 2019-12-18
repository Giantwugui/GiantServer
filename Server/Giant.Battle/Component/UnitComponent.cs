using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class UnitComponent : InitSystem, IUpdate
    {
        private readonly Dictionary<int, Unit> unitList = new Dictionary<int, Unit>();
        public Dictionary<int, Unit> UnitList => unitList;

        public override void Init()
        {
        }

        public bool AddUnit(Unit unit)
        {
            Unit exUnit = GetUnit(unit.Id);

            if (exUnit != null) return false;

            unitList[unit.Id] = unit;

            return true;
        }

        public Unit GetUnit(int id)
        {
            unitList.TryGetValue(id, out var unit);
            return unit;
        }

        public void RemoveUnit(int id)
        {
            if (unitList.TryGetValue(id, out var unit))
            {
                unitList.Remove(id);
            }
        }

        public void Update(double dt)
        {
            foreach (var kv in unitList)
            {
                kv.Value.Update(dt);
            }
        }
    }
}
