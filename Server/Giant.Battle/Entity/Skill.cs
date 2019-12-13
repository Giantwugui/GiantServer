using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public class Skill : Entity, IInitSystem<SkillModel>
    {
        public SkillModel SkillModel { get; private set; }

        public void Init(SkillModel model)
        {
            SkillModel = model;
        }

        public override void Dispose()
        {
        }
    }
}
