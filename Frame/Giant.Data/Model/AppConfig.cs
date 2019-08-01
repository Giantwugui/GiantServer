using Giant.Share;

namespace Giant.Data
{
    public class AppConfig
    {
        public AppType AppType { get; set; }
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string InnerAddress { get; set; }
        public string OutterAddress { get; set; }
    }
}