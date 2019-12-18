using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class UnitComponent : Entity, IInitSystem, IUpdate
    {
        private readonly Dictionary<int, Unit> units = new Dictionary<int, Unit>();

        private readonly Dictionary<int, Unit> playerList = new Dictionary<int, Unit>();
        public readonly Dictionary<int, Unit> PlayerList => playerList;

        public void Init()
        {
        }

        public void AddUnit(Unit unit)
        {
            Unit exUnit = GetUnit(unit.InstanceId);

            if (exUnit != null) return;

            units[unit.Id] = unit;

            AddChild(unit);
        }

        public Unit GetUnit(long instanceId) => GetChild<Unit>(instanceId);
        public void RemoveUnit(long instanceId) => RemoveChild(instanceId);

        public void Update(double dt)
        {
            foreach (var kv in units)
            {
                kv.Value.Update(dt);
            }
        }
    }
}
