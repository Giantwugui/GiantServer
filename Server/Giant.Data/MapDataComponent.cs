using Giant.Core;
using System.Collections.Generic;

namespace Giant.Data
{
    public class MapDataComponent : InitSystem, ILoad
    {
        private readonly Dictionary<int, MapModel> maps = new Dictionary<int, MapModel>();
        public Dictionary<int, MapModel> Maps => maps;

        public override void Init()
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
