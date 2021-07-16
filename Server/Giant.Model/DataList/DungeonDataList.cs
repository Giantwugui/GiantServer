using Giant.Core;

namespace Giant.Model
{
    public class DungeonDataList : SingleDataComponent<DungeonDataList, DungeonModel>
    {
        public override void Load()
        {
            Load("Dungeon");
        }
    }
}
