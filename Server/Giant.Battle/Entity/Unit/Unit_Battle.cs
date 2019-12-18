using System.Collections.Generic;

namespace Giant.Battle
{
    partial class Unit : IBattleAction
    {
        public void UpdateHP(int hp)
        {
            int value = GetComponent<NumericalComponent>().AddValue(NumericalType.HP, hp);
            if (value <= 0)
            {
                IsDead = true;
                Dead();
            }
        }


        public bool CastSkill(int skillId)
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

        public void Hit(Unit target, int damage)
        {
        }

        public void Damage(int damage)
        {
            msgSource.OnDamage(this, damage);
        }

        public void AddBuff(List<int> buffList)
        {
            msgSource.OnAddBuff(this, buffList);
        }

        public void RemoveBuff(List<int> buffList)
        {
            msgSource.OnRemoveBuff(this, buffList);
        }

        public void NumericalChange(NumericalType type, int value)
        {
            msgSource.OnNumericalChange(this, type, value);
        }

        public virtual void Dead()
        {
            msgSource.OnDead(this);
        }

        public void Relive()
        {
            msgSource.OnRelive(this);
        }
    }
}
