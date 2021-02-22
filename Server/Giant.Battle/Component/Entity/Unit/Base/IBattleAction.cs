using UnityEngine;

namespace Giant.Battle
{
    public interface IBattleAction
    {
        bool CastSkill(int skillId, int targetId, Vector2 direction);
    }
}
