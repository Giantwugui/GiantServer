using Giant.Core;

namespace Giant.Model
{
    public class MonsterModel : IData<MonsterModel>
    {
        public int Id { get; private set; }
        public int DungeonId { get; private set; }
        public int MaxHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            DungeonId = data.GetInt("DungeonId");
            Attack = data.GetInt("Attack");
            MaxHP = data.GetInt("MaxHP");
            Defense = data.GetInt("Defense");
        }
    }
}
