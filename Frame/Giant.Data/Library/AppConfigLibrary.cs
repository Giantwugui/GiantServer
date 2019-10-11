using Giant.Core;
using Giant.Share;
using System.Linq;

namespace Giant.Data
{
    public class AppConfigLibrary
    {
        private static readonly ListMap<AppType, AppConfig> appConfigs = new ListMap<AppType, AppConfig>();

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
                    AppType = EnumHelper.FromString<AppType>(data.GetString("Name")),
                    AppId = data.GetInt("AppId"),
                    SubId = data.GetInt("SubId"),
                    InnerAddress = data.GetString("InnerAddress"),
                    OutterAddress = data.GetString("OutterAddress"),
                };

                appConfigs.Add(config.AppType, config);
            }
        }

        public static AppConfig GetNetConfig(AppType appType)
        {
            appConfigs.TryGetValue(appType, out var config);
            return config?.FirstOrDefault();
        }

        public static AppConfig GetNetConfig(AppType appType, int appId, int subId = 0)
        {
            appConfigs.TryGetValue(appType, out var config);
            return config?.Where(x => x.AppId == appId && x.SubId == subId).FirstOrDefault();
        }

        //public static List<AppConfig> GetNetConfig(AppType appyType)
        //{
        //    if (!appConfigs .TryGetValue(appyType, out var topology))
        //    {
        //        Logger.Error($"Xml error, have no this AppType {appyType.ToString()}'s Netconfig");
        //    }
        //    return topology;
        //}

    }
}