using EpPathFinding;
using Giant.Core;
using Giant.EnumUtil;
using JumpPointSearch;
using UnityEngine;

namespace Giant.Model
{
    public class MapModel : IData
    {
        public int Id { get; private set; }
        public MapType MapType { get; private set; }
        public AOIType AOIType { get; private set; }
        public string MapName { get; private set; }
        public string FileName { get; private set; }

        public int MinX { get; private set; }
        public int MinY { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }
        public Vector2 BeginPosition { get; private set; }
        public bool HighPrecision { get; private set; }

        private GeoMapModel geoModel;
        public BaseGrid Grid { get { return geoModel.OldGridSmall; } }
        public BaseGrid GridBig { get { return geoModel.OldGridBig; } }
        public JumpPointParam JpParam { get { return geoModel.OldJpsFinderSmall; } }
        public JumpPointParam JpParamBig { get { return geoModel.OldJpsFinderBig; } }

        public JpsPathFinder JpsPathFinder { get { return geoModel.NewJpsFinderSmall; } }
        public JpsPathFinder JpsPathFinderBig { get { return geoModel.NewJpsFinderBig; } }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            MapType = (MapType)data.GetInt("MapType");
            AOIType = (AOIType)data.GetInt("AOIType");
            MapName = data.GetString("MapName");
            FileName = data.GetString("FileName");

            MinX = data.GetInt("MinX");
            MinY = data.GetInt("MinY");
            MaxX = data.GetInt("MaxX");
            MaxY = data.GetInt("MaxY");

            HighPrecision = data.GetBool("HighPrecision");
            string[] beginPos = data.GetString("BeginPosition").Split(':');
            BeginPosition = beginPos.Length == 2 ? new Vector2(int.Parse(beginPos[0]), int.Parse(beginPos[1])) : new Vector2(Vector2.zero.x, Vector2.zero.y);

            geoModel = MapGridDataList.Instance.GetGrid(FileName);
        }
    }
}
