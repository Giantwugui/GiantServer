namespace Giant.Core
{
    public partial class AppConfig : IData<AppConfig>
    {
        public int Id { get; private set; }
        public AppType AppType { get; private set; }
        public int AppId { get; private set; }
        public int SubId { get; private set; }
        public string InnerAddress { get; private set; }
        public string OutterAddress { get; private set; }
        public int HttpPort { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            AppType = EnumHelper.FromString<AppType>(data.GetString("Name"));
            AppId = data.GetInt("AppId");
            SubId = data.GetInt("SubId");
            InnerAddress = data.GetString("InnerAddress");
            OutterAddress = data.GetString("OutterAddress");
            HttpPort = data.GetInt("HttpPort");
        }
    }
}