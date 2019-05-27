using Giant.Share;

namespace Giant.Data
{
    public class NetConfig
    {
        public AppyType AppyType { get; set; }
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
    }

    public class ServerConfig
    {
        private static readonly DepthMap<AppyType, int, NetConfig> netTopology = new DepthMap<AppyType, int, NetConfig>();

        public static DepthMap<AppyType, int, NetConfig> NetTopology => netTopology;

        public static NetConfig GetNetConfig(AppyType appyType, int sunId)
        {
            netTopology.TryGetValue(appyType, sunId, out var config);
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
                    AppyType = (AppyType)data.GetInt("AppType"),
                    AppId = data.GetInt("AppId"),
                    SubId = data.GetInt("SubId"),
                    IP = data.GetString("IP"),
                    Port = data.GetInt("Port"),
                };

                netTopology.Add(config.AppyType, config.SubId, config);
            }
        }

    }
}