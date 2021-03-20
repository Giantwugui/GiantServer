using UnityEngine;

namespace Giant.Battle
{
    /**
     * 用于前端用户事件
     */
    interface IClientAction
    {
        /// <summary>
        /// 传送
        /// </summary>
        /// <param name="vector"></param>
        void Transmit(Vector2 vector);

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="vector"></param>
        void SetDestination(Vector2 vector);

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="targetId"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        bool CastSkill(int skillId, int targetId, Vector2 pos, Vector2 direction);
    }
}
