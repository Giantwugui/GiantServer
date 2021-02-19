using Giant.Core;

namespace Giant.Model
{
    public class DungeonLibrary : SingleDataComponent<DungeonLibrary, DungeonModel>
    {
        public override void Load()
        {
            Load("Dungeon");
        }
    }
}
