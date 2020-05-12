using UnityEngine;

namespace Giant.Battle
{
    public interface IBattleAction
    {
        void OnHit(Unit target, int damage);
        void Damage(int damage);
        bool CastSkill(int skillId, int targetId, Vector2 direction);
        void AddBuff(int buffId);
        void RemoveBuff(int buffId);
        void NatureChange(NatureType type, int value);
        void OnDead();
        void OnRelive();
    }
}
