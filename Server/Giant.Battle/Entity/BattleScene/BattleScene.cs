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
            Scene.EventSystem.Handle(EventType.BattleStart, this);
        }

        public virtual void Stop(BattleResult result)
        {
            Scene.EventSystem.Handle(EventType.BattleStop, this, result);
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
            Scene.EventSystem.Handle(EventType.UnitEnterScene, this, unit);
        }

        public void Leave(int id)
        {
            Scene.EventSystem.Handle(EventType.UnitLeaveScene, this, id);
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
