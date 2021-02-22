using Giant.EnumUtil;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit
    {
        public SkillComponent SkillComponent { get; private set; }

        protected virtual void InitSkill() 
        {
            SkillComponent = AddComponentWithParent<SkillComponent>();
        }

        public bool CastSkill(int skillId, int targetId, Vector2 pos, Vector2 direction)
        {
            Skill skill = SkillComponent.GetSkill(skillId);
            if (skill == null) return false;

            if (!SkillComponent.Check(skill))
            {
                return false;
            }

            //TODO 释放技能过程中，新技能释放的机制

            skill.InitSkillCastParam(pos, direction, targetId);
            FsmComponent.SetNextFsmState(FsmType.Skill, skill);

            return true;
        }

        /// <summary>
        /// 是否可以普攻
        /// </summary>
        /// <returns></returns>
        public bool CanCastNormaAttack()
        {
            if (InBuffState(BuffType.Dizzy))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否可以释放技能
        /// </summary>
        /// <returns></returns>
        public bool CanCastNormaSkill()
        {
            if (InBuffState(BuffType.Dizzy))
            {
                return false;
            }
            return true;
        }

        public void SkillEffect(Skill skill)
        {
            //TODO

            SkillComponent.AfterCasting(skill);
        }
    }
}
