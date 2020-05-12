using Giant.Core;
using System.Collections.Generic;

namespace Giant.Data
{
    public class DungeonLibComponent : SingleDataComponent<DungeonLibComponent, DungeonModel>
    {
        public override void Load()
        {
            Load("Dungeon");
        }
    }
}
