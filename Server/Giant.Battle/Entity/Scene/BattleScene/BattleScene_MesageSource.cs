using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    partial class BattleScene : IBattleMsgSource
    {
        #region 广播

        public void OnBattleStart()
        {
            PlayerList.ForEach(x => x.Value.MsgListener.OnBattleStart());
        }

        public void OnBattleStop(MapModel model, BattleResult result)
        {
            PlayerList.ForEach(x => x.Value.MsgListener.OnBattleStop(model, result));
        }

        public void OnBattleEnd()
        {
            PlayerList.ForEach(x => x.Value.MsgListener.OnBattleEnd());
        }


        public void OnUnitEnter(Unit unit) 
            => Scene.EventSystem.Handle(EventType.UnitEnterScene, this, unit);

        public void OnUnitLeave(Unit unit)
            =>Scene.EventSystem.Handle(EventType.UnitLeaveScene, this, unit);

        public void OnDamage(Unit unit, int damage)
            =>Scene.EventSystem.Handle(EventType.Damage, this, unit, damage);

        public void OnCastSkill(Unit unit, int skillId)
            =>Scene.EventSystem.Handle(EventType.UnitCastSkill, this, unit, skillId);

        public void OnAddBuff(Unit unit, int buffId)
            =>Scene.EventSystem.Handle(EventType.UnitAddBuff, this, unit, buffId);

        public void OnRemoveBuff(Unit unit, int buffId)
            =>Scene.EventSystem.Handle(EventType.UnitRemoveBuff, this, unit, buffId);

        public void OnNatureChange(Unit unit, NatureType type, int value)
            =>Scene.EventSystem.Handle(EventType.NumbercalChange, this, unit, type, value);

        public void OnDead(Unit unit)
            =>Scene.EventSystem.Handle(EventType.UnitDead, this, unit);

        public void OnRelive(Unit unit)
            =>Scene.EventSystem.Handle(EventType.UnitRelive, this, unit);

        public void OnMove(Unit unit, Vector2 vector)
            =>Scene.EventSystem.Handle(EventType.PosChange, this, unit, vector);

        #endregion
    }
}
