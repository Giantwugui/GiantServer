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
            Broadcast_BattleStart();
        }

        public virtual void Stop()
        {
        }

        public virtual void Close()
        {
        }

        public override void Dispose()
        {
        }

        #region action

        public void Enter(Unit unit)
        {
            UnitComponent.AddUnit(unit);
        }

        public void Leave(long instanceId)
        {
            UnitComponent.RemoveUnit(instanceId);
        }

        public void Move(long instanceId, Vector2 vector)
        {
            UnitComponent.GetUnit(instanceId)?.Move(vector);
        }

        public void CastSkill(long instanceId, int skillId)
        {
            UnitComponent.GetUnit(instanceId)?.CaskSkill(skillId);
        }

        #endregion
    }
}
