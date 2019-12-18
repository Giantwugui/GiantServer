using Giant.Msg;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    partial class BattleScene : IBattleMsgSource
    {
        private void BroadCastMsg(Google.Protobuf.IMessage message)
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
        {
            unit.MsgListener.OnEnterBattleScene(this);
        }

        public void OnUnitLeave(Unit unit)
        {
            unit.MsgListener.OnLeaveBattleScene();
        }

        public void OnDamage(Unit unit, int damage)
        {
        }

        public void OnCastSkill(Unit unit, int skillId)
        {
        }

        public void OnAddBuff(Unit unit, List<int> buffList)
        {
        }

        public void OnRemoveBuff(Unit unit, List<int> buffList)
        {
        }

        public void OnNumericalChange(Unit unit, NumericalType type, int value)
        {
        }

        public void OnDead(Unit unit)
        {
        }

        public void OnRelive(Unit unit)
        {
        }

        public void OnMove(Unit unit, Vector2 vector)
        {
        }

        #endregion
    }
}
