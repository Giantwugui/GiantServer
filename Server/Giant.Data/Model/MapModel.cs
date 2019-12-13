using Giant.Core;

namespace Giant.Data
{
    public class MapModel : IData<MapModel>
    {
        public int Id { get; private set; }
        public string MapName { get; private set; }
        public string BianryName { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            MapName = data.GetString("MapName");
            BianryName = data.GetString("BianryName");
        }
    }
}
