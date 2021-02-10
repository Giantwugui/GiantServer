using Giant.Core;
using System.Collections.Generic;

namespace Giant.Model
{
    public class MapLibComponent : SingleDataComponent<MapLibComponent, MapModel>
    {
        public Dictionary<int, MapModel> Maps => Models;

        public override void Load()
        {
            Load("Map");
        }
    }
}
