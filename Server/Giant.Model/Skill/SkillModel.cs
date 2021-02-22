using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Model
{
    public class SkillModel : IData
    {
        public int Id { get; private set; }
        public int SkillType { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            SkillType = data.GetInt("SkillType");
        }
    }
}
