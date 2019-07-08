namespace Giant.Data
{
    public class MapModel
    {
        public int MapId { get; private set; }
        public string MapName { get; private set; }
        public string BianryName { get; private set; }

        public Data Data { get; private set; }


        public MapModel(Data data)
        {
            this.Data = data;
            this.BindData();
        }

        private void BindData()
        {
            var data = this.Data;
            this.MapId = data.Id;
            this.MapName = data.GetString("MapName");
            this.BianryName = data.GetString("BianryName");
        }
    }
}
