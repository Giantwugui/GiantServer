using Giant.Log;
using Giant.Share;
using System.Collections.Generic;

namespace Giant.Data
{
    public class NetConfig
    {
        public AppType AppyType { get; set; }
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string InnerAddress { get; set; }
        public string OutterAddress { get; set; }
    }

    public class ServerConfig
    {
        private static readonly DepthMap<AppType, int, NetConfig> netConfigs= new DepthMap<AppType, int, NetConfig>();

        public static NetConfig GetNetConfig(AppType appyType, int sunId)
        {
            netConfigs.TryGetValue(appyType, sunId, out var config);
            return config;
        }

        public static void Init()
        {
            Data data;
            NetConfig config;
            var datas = DataManager.Instance.GetDatas("ServerConfig");
            foreach (var kv in datas)
            {
                data = kv.Value;
                config = new NetConfig()
                {
                    AppyType = (AppType)data.GetInt("AppType"),
                    AppId = data.GetInt("AppId"),
                    SubId = data.GetInt("SubId"),
                    InnerAddress = data.GetString("InnerAddress"),
                    OutterAddress = data.GetString("OutterAddress"),
                };

                netConfigs.Add(config.AppyType, config.SubId, config);
            }
        }

        public static Dictionary<int, NetConfig> GetNetConfig(AppType appyType)
        {
            if (!netConfigs.TryGetValue(appyType, out var topology))
            {
                Logger.Error($"Xml error, have no this AppType {appyType.ToString()}'s Netconfig");
            }
            return topology;
        }

    }
}