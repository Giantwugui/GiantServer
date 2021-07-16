using Giant.Core;
using System.Collections.Generic;

namespace Giant.Model
{
    public class MapDataList : SingleDataComponent<MapDataList, MapModel>
    {
        public Dictionary<int, MapModel> Maps => Models;

        public override void Load()
        {
            Load("Map");
        }
    }
}
