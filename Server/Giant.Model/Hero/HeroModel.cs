using Giant.Core;

namespace Giant.Model
{
    public class HeroModel : IData
    {
        public int Id { get; private set; }
        public int Attack { get; private set; }
        public int HP { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            Attack = data.GetInt("Attack");
            HP = data.GetInt("HP");
        }
    }
}
