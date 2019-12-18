using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public class Skill : Entity, IInitSystem<SkillModel>
    {
        private Unit owner;
        private int skillCastEnergy;

        public int Id { get; private set; }
        public int SkillEnergy { get; private set; }
        public SkillType SkillType { get; private set; }

        public void Init(SkillModel model)
        {
            Id = model.Id;
            SkillType = (SkillType)model.SkillType;

            owner = GetParent<SkillComponent>().GetParent<Unit>();
        }

        public bool CheckCast()
        {
            return SkillEnergy >= skillCastEnergy;
        }

        public void Start()
        {
        }

        public void End()
        {
        }

        public override void Dispose()
        {
        }
    }
}
