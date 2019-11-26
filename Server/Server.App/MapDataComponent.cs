using Giant.Core;
using Giant.Framework;
using System.Collections.Generic;

namespace Server.App
{
    public class MapDataComponent : IInitSystem, ILoadSystem
    {
        private Dictionary<int, MapModel> maps = new Dictionary<int, MapModel>();
        public Dictionary<int, MapModel> Maps => maps;

        public void Init()
        {
            maps.Clear();

            MapModel model;
            var datas = DataComponent.Instance.GetDatas("Map");
            foreach (var kv in datas)
            {
                model = new MapModel(kv.Value);
                maps.Add(model.MapId, model);
            }
        }

        public void Load()
        {
            Init();
        }
    }
}
