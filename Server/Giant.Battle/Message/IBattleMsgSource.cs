using Google.Protobuf;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public interface IBattleMsgSource
    {
        void OnBattleStart();
        void OnBattleStop(BattleResult result);

        void OnUnitEnter(Unit unit);
        void OnUnitLeave(Unit unit);

        void OnMove(Unit unit, Vector2 vector);
        void OnDamage(Unit unit, int damage);
        void OnCastSkill(Unit unit, int skillId);
        void OnAddBuff(Unit unit, List<int> buffList);
        void OnRemoveBuff(Unit unit, List<int> buffList);
        void OnNumericalChange(Unit unit, NumericalType type, int value);
        void OnDead(Unit unit);
        void OnRelive(Unit unit);
    }
}
