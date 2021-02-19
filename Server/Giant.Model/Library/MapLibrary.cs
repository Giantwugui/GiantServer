using Giant.Core;
using System.Collections.Generic;

namespace Giant.Model
{
    public class MapLibrary : SingleDataComponent<MapLibrary, MapModel>
    {
        public Dictionary<int, MapModel> Maps => Models;

        public override void Load()
        {
            Load("Map");
        }
    }
}
