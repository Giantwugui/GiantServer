using Giant.Core;
using Giant.Data;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class SkillComponent : InitSystem<List<Skill>>
    {
        private static List<Skill> skills;

        public override void Init(List<Skill> skillList)
        {
            skills = new List<Skill>(skillList);
        }

        public void AddSkill(int skillId)
        {
            SkillModel model = SkillDataComponent.Instance.GetModel(skillId);
        }
    }
}
