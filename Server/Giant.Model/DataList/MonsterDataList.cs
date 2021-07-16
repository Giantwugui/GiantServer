using Giant.Core;
using System.Collections.Generic;

namespace Giant.Model
{
    public class MonsterDataList : SingleDataComponent<MonsterDataList, MonsterModel>
    {
        private readonly Dictionary<int, List<MonsterModel>> dungeonMonsters = new Dictionary<int, List<MonsterModel>>();

        public override void Load()
        {
            dungeonMonsters.Clear();

            //Load("Dungeon");

            foreach (var kv in Models)
            {
                dungeonMonsters.TryGetValue(kv.Value.DungeonId, out var monsters);
                if (monsters == null)
                {
                    monsters = new List<MonsterModel>();
                    dungeonMonsters.Add(kv.Value.DungeonId, monsters);
                }
                monsters.Add(kv.Value);
            }
        }

        public List<MonsterModel> GetMonsterModels(int dungeonId)
        {
            dungeonMonsters.TryGetValue(dungeonId, out var monsters);
            return monsters;
        }
    }
}
