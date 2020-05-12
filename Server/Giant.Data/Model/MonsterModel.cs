using Giant.Core;

namespace Giant.Data
{
    public class MonsterModel : IData<MonsterModel>
    {
        public int Id { get; private set; }
        public int DungeonId { get; private set; }
        public int Attack { get; private set; }
        public int HP { get; private set; }

        public void Bind(Core.DataModel data)
        {
            Id = data.Id;
            DungeonId = data.GetInt("DungeonId");
            Attack = data.GetInt("Attack");
            HP = data.GetInt("HP");
        }
    }
}
