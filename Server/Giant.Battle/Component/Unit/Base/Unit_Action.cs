using Giant.EnumUtil;
using Giant.Msg;
using UnityEngine;

namespace Giant.Battle
{
    /*
     * Unit 事件统一处理文件
     * 1.用于消息的广播
     * 2.后续事件（trigger）的触发
     */
    partial class Unit
    {
        public void OnDamage(Unit caster, int damage)
        {
        }

        public void OnSkillStart(Skill skill)
        {
            MSG_ZGC_SKILL_START msg = BuildSkillStartMessage(skill);
            Broadcast(msg);
        }

        public void OnSkillEnd(Skill skill)
        {
        }

        public void OnAddBuff(int buffId)
        {
        }

        public void OnRemoveBuff(int buffId)
        {
        }

        public void OnNatureChange(NatureType type, int value)
        {
        }

        public void OnDead()
        {
        }

        public void OnRelive()
        {
        }

        public void OnMove(Vector2 vector)
        {
        }
    }
}
