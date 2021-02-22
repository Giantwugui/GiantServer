using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Model
{
    public class SkillModel : IData
    {
        public int Id { get; private set; }
        public SkillType SkillType { get; private set; }
        public int Priority { get; private set; }
        public int Energy { get; private set; }

        public float EffectTime { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            SkillType = (SkillType)data.GetInt("SkillType");
            Priority = data.GetInt("Priority");
            Energy = data.GetInt("Energy");
            EffectTime = data.GetFloat("Energy");
        }
    }
}
