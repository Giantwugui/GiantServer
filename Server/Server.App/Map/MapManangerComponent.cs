using Giant.Core;
using System.Collections.Generic;

namespace Server.App
{
    public class MapManangerComponent : Component, IInitSystem<MapDataComponent>
    {
        private Dictionary<int, MapComponent> maps = new Dictionary<int, MapComponent>();

        public MapComponent GetMap(int mapId)
        {
            maps.TryGetValue(mapId, out var map);
            return map;
        }

        public void Init(MapDataComponent dataList)
        {
            MapComponent map;
            dataList.Maps.ForEach(kv =>
            {
                map = ComponentFactory.CreateComponent<MapComponent, MapModel>(kv.Value);
                maps.Add(map.MapId, map);
            });
        }
    }
}
