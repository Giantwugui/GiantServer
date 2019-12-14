using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public class Skill : Entity, IInitSystem<SkillModel>
    {
        private Unit owner;

        public int Id { get; private set; }
        public SkillType SkillType { get; private set; }

        public void Init(SkillModel model)
        {
            Id = model.Id;
            SkillType = (SkillType)model.SkillType;

            owner = GetParent<SkillComponent>().GetParent<Unit>();
        }

        public override void Dispose()
        {
        }
    }
}
