using UnityEngine;

namespace Giant.Battle
{
    /**
     * 用于前端用户事件
     */
    interface IClientAction
    {
        bool Move(Vector2 vector);
        bool CastSkill(int skillId, int targetId, Vector2 pos, Vector2 direction);
    }
}
