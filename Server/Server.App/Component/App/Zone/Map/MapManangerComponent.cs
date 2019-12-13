using Giant.Core;
using Giant.Data;
using System.Collections.Generic;

namespace Server.App
{
    public class MapManangerComponent : InitSystem<MapDataComponent>
    {
        private Dictionary<int, MapComponent> maps = new Dictionary<int, MapComponent>();

        public static MapManangerComponent Instance { get; private set; }

        public override void Init(MapDataComponent dataList)
        {
            Instance = this;
            MapComponent map;
            dataList.Maps.ForEach(kv =>
            {
                map = ComponentFactory.CreateComponent<MapComponent, MapModel>(kv.Value);
                maps.Add(map.MapId, map);
            });
        }

        public MapComponent GetMap(int mapId)
        {
            maps.TryGetValue(mapId, out var map);
            return map;
        }
    }
}
