using CommandLine;
using Giant.Share;

namespace Giant.Frame
{
    public class AppOption
    {
        [Option("appType", Required = true, Default = AppType.Manager)]
        public AppType AppType { get; set; }

        [Option("appId", Required = true, Default = 0)]
        public int AppId { get; set; }
    }
}
