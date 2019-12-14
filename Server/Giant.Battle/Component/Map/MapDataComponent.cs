using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class MapDataComponent : SingleDataComponent<MapDataComponent, MapModel>
    {
        public Dictionary<int, MapModel> Maps => Models;

        public override void Load()
        {
            Load("Map");
        }
    }
}
