using Giant.Data;
using Giant.Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.App
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
