using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Data
{
    public class MapLibrary
    {
        private static Dictionary<int, MapModel> maps = new Dictionary<int, MapModel>();

        public static Dictionary<int, MapModel> Maps => maps;

        public static void Init()
        {
            MapModel model;
            var datas = DataManager.Instance.GetDatas("Map");
            foreach (var kv in datas)
            {
                model = new MapModel(kv.Value);
                maps.Add(model.MapId, model);
            }
        }
    }
}
