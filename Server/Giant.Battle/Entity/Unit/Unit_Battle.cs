using System.Collections.Generic;

namespace Giant.Battle
{
    partial class Unit : IBattleMsgSource
    {
        public void UpdateHP(int hp)
        {
            int value = GetComponent<NumericalComponent>().AddValue(NumericalType.HP, hp);
            if (value <= 0)
            {
                IsDead = true;
                OnDead();
            }
        }


        public bool CaskSkill(int skillId)
        {
            Skill skill = GetComponent<SkillComponent>().GetSkill(skillId);

            if (skill == null) return false;

            if (!skill.CheckCast()) return false;

            if (!CheckSkillAttack(skill.SkillType)) return false;

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

        public void OnHit()
        {
        }

        public void OnDamage(int damage)
        {
        }

        public void OnCastSkill(int skillId)
        {
        }

        public void OnAddBuff(List<int> buffList)
        {
        }

        public void OnRemoveBuff(List<int> buffList)
        {
        }

        public void OnNumericalChange(NumericalType type, int value)
        {
        }

        public virtual void OnDead()
        {
        }

        public void OnRelive()
        {
        }
    }
}
