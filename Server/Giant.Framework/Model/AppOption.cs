using CommandLine;
using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Framework
{
    public class AppOption
    {
        [Option("appType", Required = true, Default = AppType.AllServer)]
        public AppType AppType { get; set; }

        [Option("appId", Required = true, Default = 0)]
        public int AppId { get; set; }

        [Option("subId", Required = false, Default = 0)]
        public int SubId { get; set; }
    }
}
