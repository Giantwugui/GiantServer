using CommandLine;
using Giant.Share;

namespace Server.Frame
{
    public class AppOption
    {
        [Option("appType", Required = true, Default = AppType.AllServer)]
        public AppType AppType { get; set; }

        [Option("appId", Required = true, Default = 0)]
        public int AppId { get; set; }
    }
}
