using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class BattleScene : Entity, IInitSystem<MapModel>, IUpdate
    {
        public UnitComponent UnitComponent => GetComponent<UnitComponent>();

        public void Init(MapModel model)
        {
            AddComponentWithParent<MapComponent, MapModel>(model);
            AddComponentWithParent<UnitComponent>();
        }

        public void Update(double dt)
        {
            UnitComponent.Update(dt);
        }

        public virtual void Start()
        {
            OnBattleStart();
        }

        public virtual void Stop(BattleResult result)
        {
            OnBattleStop(result);
        }

        public void Close()
        {
        }

        public override void Dispose()
        {
        }

        #region action

        public void Enter(Unit unit)
        {
            if (UnitComponent.AddUnit(unit))
            {
                OnUnitEnter(unit);
            }
        }

        public void Leave(int id)
        {
            Unit unit = UnitComponent.GetUnit(id);
            if (unit == null) return;

            UnitComponent.RemoveUnit(id);

            OnUnitLeave(unit);
        }

        public void Move(int id, Vector2 vector)
        {
            UnitComponent.GetUnit(id)?.Move(vector);
        }

        public void CastSkill(int id, int skillId)
        {
            UnitComponent.GetUnit(id)?.CastSkill(skillId);
        }

        #endregion
    }
}
