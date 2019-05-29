using Giant.Log;
using Giant.Share;
using System.Collections.Generic;

namespace Giant.Data
{
    public class NetConfigModel
    {
        public AppType AppyType { get; set; }
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string InnerAddress { get; set; }
        public string OutterAddress { get; set; }
    }

    public class NetConfig
    {
        private static readonly DepthMap<AppType, int, NetConfigModel> netConfigs= new DepthMap<AppType, int, NetConfigModel>();

        public static NetConfigModel GetNetConfig(AppType appyType, int sunId)
        {
            netConfigs.TryGetValue(appyType, sunId, out var config);
            return config;
        }

        public static void Init()
        {
            Data data;
            NetConfigModel config;
            var datas = DataManager.Instance.GetDatas("NetConfig");
            foreach (var kv in datas)
            {
                data = kv.Value;
                config = new NetConfigModel()
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

        public static Dictionary<int, NetConfigModel> GetNetConfig(AppType appyType)
        {
            if (!netConfigs.TryGetValue(appyType, out var topology))
            {
                Logger.Error($"Xml error, have no this AppType {appyType.ToString()}'s Netconfig");
            }
            return topology;
        }

    }
}