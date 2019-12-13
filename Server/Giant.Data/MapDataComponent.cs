using Giant.Core;
using System.Collections.Generic;

namespace Giant.Data
{
    public class MapDataComponent : SingleDataComponent<MapDataComponent, MapModel>
    {
        public Dictionary<int, MapModel> Maps => Models;

        public override void Load()
        {
            Clear();
            Load("Map");
        }
    }
}
