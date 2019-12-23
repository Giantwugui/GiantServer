using Giant.Core;
using Giant.Msg;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    partial class BattleScene : IBattleMsgSource
    {
        public void BroadCastMsg(Google.Protobuf.IMessage message)
        {
            foreach (var kv in UnitComponent.UnitList)
            {
                kv.Value.BroadCast(message);
            }
        }

        private void BroadCastMsgExceptUnit(Google.Protobuf.IMessage message, int id)
        {
            foreach (var kv in UnitComponent.UnitList)
            {
                if (kv.Key == id) continue;

                kv.Value.BroadCast(message);
            }
        }

        #region 广播

        public void OnBattleStart()
        {
            Msg_ZGate_Battle_Start msg = new Msg_ZGate_Battle_Start();
            BroadCastMsg(msg);
        }

        public void OnBattleStop(BattleResult result)
        {
            foreach (var kv in UnitComponent.UnitList)
            {
                kv.Value.MsgListener.OnStopBattle(result);
            }
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

        public void OnNumericalChange(Unit unit, NumericalType type, int value)
            =>Scene.EventSystem.Handle(EventType.NumeercalChange, this, unit, type, value);

        public void OnDead(Unit unit)
            =>Scene.EventSystem.Handle(EventType.UnitDead, this, unit);

        public void OnRelive(Unit unit)
            =>Scene.EventSystem.Handle(EventType.UnitRelive, this, unit);

        public void OnMove(Unit unit, Vector2 vector)
            =>Scene.EventSystem.Handle(EventType.PosChange, this, unit, vector);

        #endregion
    }
}
