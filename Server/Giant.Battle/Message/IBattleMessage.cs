using Google.Protobuf;
using System.Collections.Generic;

namespace Giant.Battle
{
    public interface IBattleMsgSource
    {
        void OnHit();
        void OnDamage(int damage);
        void OnCastSkill(int skillId);
        void OnAddBuff(List<int> buffList);
        void OnRemoveBuff(List<int> buffList);
        void OnNumericalChange(NumericalType type, int value);
        void OnDead();
        void OnRelive();
    }

    public interface IBattleMsgListener
    {
        void BroadCastBattleMsg(IMessage message);
    }
}
