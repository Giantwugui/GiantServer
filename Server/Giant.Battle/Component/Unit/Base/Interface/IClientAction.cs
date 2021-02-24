using UnityEngine;

namespace Giant.Battle
{
    /**
     * 用于前端用户事件
     */
    interface IClientAction
    {
        void Transmit(Vector2 vector);
        void SetDestination(Vector2 vector);
        bool CastSkill(int skillId, int targetId, Vector2 pos, Vector2 direction);
    }
}
