using Giant.Core;
using System.Collections.Generic;

namespace Giant.Model
{
    /// <summary>
    /// 地形信息
    /// </summary>
    public class MapGridDataList : SingleComponent<MapGridDataList>
    {
        private Dictionary<string, GeoMapModel> mapGridList = new Dictionary<string, GeoMapModel>();

        public override void Init()
        {
            mapGridList.Clear();
            foreach (var kv in DataComponent.Instance.GetDatas("Map"))
            {
                string fileName = kv.Value.GetString("FileName");
                LoadMapData(fileName);
            }
        }

        public GeoMapModel GetGrid(string name)
        {
            mapGridList.TryGetValue(name, out var grid);
            return grid;
        }

        private void LoadMapData(string fileName)
        {
            /*
             EPPathFind，a*寻路
             */
            //DynamicGrid grid = new DynamicGrid();
            //using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //using (BinaryReader reader = new BinaryReader(fileStream))
            //{
            //    int length = reader.ReadInt32();
            //    for (int i = 0; i < length; ++i)
            //    {
            //        grid.SetWalkableAt(new GridPos(reader.ReadInt32(), reader.ReadInt32()), reader.ReadBoolean());
            //    }
            //}

            /*
             * 跳点寻路
             */
            GeoMapModel geoMap = new GeoMapModel(fileName);
            mapGridList.Add(fileName, geoMap);
        }
    }
}
