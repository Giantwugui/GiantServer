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
        public string BianryName { get; private set; }

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
            BianryName = data.GetString("BianryName");
        }
    }
}
