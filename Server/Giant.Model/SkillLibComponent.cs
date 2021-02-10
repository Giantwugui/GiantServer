using Giant.Core;

namespace Giant.Model
{
    public class SkillLibComponent : SingleDataComponent<SkillLibComponent, SkillModel>
    {
        public override void Load()
        {
            Load("Skill");
        }
    }
}
