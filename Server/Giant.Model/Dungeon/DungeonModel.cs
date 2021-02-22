using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Model
{
    public class DungeonModel : IData
    {
        public int Id { get; private set; }
        public MapType MapType { get; private set; }
        public float DelayTime { get; private set; }
        public int DuringTime { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            MapType = (MapType)data.GetInt("MapType");
            DelayTime = data.GetFloat("DelayTime");
            DuringTime = data.GetInt("DuringTime");
        }
    }
}
