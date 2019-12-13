using Giant.Core;

namespace Giant.Data
{
    public class SkillModel : IData<SkillModel>
    {
        public int Id { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
        }
    }
}
