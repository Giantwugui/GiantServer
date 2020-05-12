using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit
    {
        protected SkillComponent SkillComponent { get; private set; }

        public bool CastSkill(int skillId, int targetId, Vector2 direction)
        {
            Skill skill = GetComponent<SkillComponent>().GetSkill(skillId);

            if (skill == null) return false;

            if (!skill.CheckCast()) return false;

            if (!CheckSkillAttack(skill.SkillType)) return false;

            msgSource.OnCastSkill(this, skillId);

            skill.Start();

            return true;
        }

        private bool CheckSkillAttack(SkillType type)
        {
            BuffComponent component = GetComponent<BuffComponent>();

            if (component == null) return false;

            //TODO buff 状态判断

            return true;
        }
    }
}
