using System.Collections.Generic;

namespace Giant.Battle
{
    public interface IBattleAction
    {
        void Hit(Unit target, int damage);
        void Damage(int damage);
        bool CastSkill(int skillId);
        void AddBuff(int buffId);
        void RemoveBuff(int buffId);
        void NumericalChange(NumericalType type, int value);
        void Dead();
        void Relive();
    }
}
