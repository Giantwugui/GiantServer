using Giant.Framework;

namespace Server.App
{
    public class MapModel
    {
        public int MapId { get; private set; }
        public string MapName { get; private set; }
        public string BianryName { get; private set; }

        public Data Data { get; private set; }


        public MapModel(Data data)
        {
            Data = data;
            BindData();
        }

        private void BindData()
        {
            var data = Data;
            MapId = data.Id;
            MapName = data.GetString("MapName");
            BianryName = data.GetString("BianryName");
        }
    }
}
