using Giant.Core;
using Giant.Model;
using Giant.Logger;
using System.Collections.Generic;
using Giant.EnumUtil;

namespace Giant.Battle
{
    public class SkillComponent : InitSystem<Unit>
    {
        private Unit owner;
        private readonly Dictionary<int, Skill> skills = new Dictionary<int, Skill>();

        public override void Init(Unit unit)
        {
            owner = unit;
        }

        public void AddSkill(int skillId)
        {
            if (skills.ContainsKey(skillId))
            {
                //TODO 是否允许，相同技能重复添加
                return;
            }

            SkillModel model = SkillDataList.Instance.GetModel(skillId);
            if (model == null)
            {
                Log.Error($"have no this skill model {skillId}");
                return;
            }

            Skill skill = AddComponentWithParent<Skill, SkillModel>(model);
            AddSkill(skill);
        }

        public void AddSkill(Skill skill)
        {
            skills.Add(skill.Id, skill);
        }

        public Skill GetSkill(int skillId)
        {
            skills.TryGetValue(skillId, out var skill);
            return skill;
        }

        public bool Check(Skill skill)
        {
            if (!skill.Check()) return false;

            switch (skill.Model.SkillType)
            {
                case SkillType.NormalAttack1:
                    {
                        if (!CheckNormalAttack(skill)) return false;
                    }
                    break;
                case SkillType.Normal:
                    {
                        if (!CheckNormalSkill(skill)) return false;
                    }
                    break;
            }
            return true;
        }

        private bool CheckNormalAttack(Skill skill)
        {
            if (!owner.CanCastNormaAttack()) return false;

            return true;
        }

        private bool CheckNormalSkill(Skill skill)
        {
            if (!owner.CanCastNormaSkill()) return false;

            return true;
        }

        public void StartCasting(Skill skill)
        {
            switch (skill.Model.SkillType)
            {
                case SkillType.NormalAttack1:
                    break;
                case SkillType.Normal:
                    skill.ResetEnergy();
                    break;
            }
        }

        public void AfterCasting(Skill skill)
        {
            skill.ResetEnergy();
        }
    }
}
