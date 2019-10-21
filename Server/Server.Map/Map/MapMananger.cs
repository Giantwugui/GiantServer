using Giant.Core;
using Giant.Data;
using System.Collections.Generic;

namespace Server.Map
{
    public class MapMananger
    {
        private Dictionary<int, Map> maps = new Dictionary<int, Map>();

        public Map GetMap(int mapId)
        {
            maps.TryGetValue(mapId, out var map);
            return map;
        }

        public void Init()
        {
            Map map = null;
            MapLibrary.Maps.ForEach(model=>
            {
                map = new Map(this, model.Value);
                maps.Add(map.MapId, map);
            });
        }
    }
}
