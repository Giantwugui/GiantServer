using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Model
{
    public class MapModel : IData
    {
        public int Id { get; private set; }
        public MapType MapType { get; private set; }
        public string MapName { get; private set; }
        public string BianryName { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            MapType = (MapType)data.GetInt("MapType");
            MapName = data.GetString("MapName");
            BianryName = data.GetString("BianryName");
        }
    }
}
