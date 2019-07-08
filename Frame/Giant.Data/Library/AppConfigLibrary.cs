using Giant.Log;
using Giant.Share;
using System.Collections.Generic;

namespace Giant.Data
{
    public class AppConfigLibrary
    {
        private static readonly Dictionary<int, AppConfig> appConfigs = new Dictionary<int, AppConfig>();
        private static readonly DepthMap<AppType, int, AppConfig> appConfigIndexByAppType= new DepthMap<AppType, int, AppConfig>();

        public static void Init()
        {
            Data data;
            AppConfig config;
            var datas = DataManager.Instance.GetDatas("AppConfig");
            foreach (var kv in datas)
            {
                data = kv.Value;
                config = new AppConfig()
                {
                    ApyType = (AppType)data.GetInt("AppType"),
                    AppId = data.GetInt("AppId"),
                    InnerAddress = data.GetString("InnerAddress"),
                    OutterAddress = data.GetString("OutterAddress"),
                };

                appConfigs.Add(config.AppId, config);
                appConfigIndexByAppType.Add(config.ApyType, config.AppId, config);
            }
        }

        public static AppConfig GetNetConfig(int appId)
        {
            appConfigs.TryGetValue(appId, out var config);
            return config;
        }

        public static Dictionary<int, AppConfig> GetNetConfig(AppType appyType)
        {
            if (!appConfigIndexByAppType.TryGetValue(appyType, out var topology))
            {
                Logger.Error($"Xml error, have no this AppType {appyType.ToString()}'s Netconfig");
            }
            return topology;
        }

    }
}