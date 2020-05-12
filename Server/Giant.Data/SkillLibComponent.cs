using Giant.Core;

namespace Giant.Data
{
    public class SkillLibComponent : SingleDataComponent<SkillLibComponent, SkillModel>
    {
        public override void Load()
        {
            Load("Skill");
        }
    }
}
