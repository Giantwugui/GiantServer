using Giant.Core;

namespace Giant.Data
{
    public class SkillDataComponent : SingleDataComponent<SkillDataComponent, SkillModel>
    {
        public override void Load()
        {
            Load("Skill");
        }
    }
}
