using Giant.Core;
using Giant.Data;
using Giant.Logger;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class SkillComponent : InitSystem
    {
        private readonly Dictionary<int, Skill> skills = new Dictionary<int, Skill>();

        public override void Init()
        {
        }

        public void AddSkill(int skillId)
        {
            if (skills.ContainsKey(skillId))
            {
                //TODO 是否允许，相同技能重复添加
                return;
            }

            SkillModel model = SkillLibComponent.Instance.GetModel(skillId);
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
    }
}
