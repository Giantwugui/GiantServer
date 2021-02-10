using Giant.Core;

namespace Giant.Model
{
    public class DungeonLibComponent : SingleDataComponent<DungeonLibComponent, DungeonModel>
    {
        public override void Load()
        {
            Load("Dungeon");
        }
    }
}
