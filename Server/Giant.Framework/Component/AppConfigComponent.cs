using Giant.Core;
using System.Linq;

namespace Giant.Framework
{
    public class AppConfigComponent : InitSystem, ILoad
    {
        private readonly ListMap<AppType, AppConfig> appConfigs = new ListMap<AppType, AppConfig>();

        public AppConfigComponent() { }

        public override void Init()
        {
            appConfigs.Clear();

            Data data;
            AppConfig config;
            var datas = DataComponent.Instance.GetDatas("AppConfig");
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
                    HttpPorts = data.GetString("HttpPort").ToIntList()
                };

                appConfigs.Add(config.AppType, config);
            }
        }

        public void Load()
        {
            Init();
        }

        public AppConfig GetNetConfig(AppType appType)
        {
            appConfigs.TryGetValue(appType, out var config);
            return config?.FirstOrDefault();
        }

        public AppConfig GetNetConfig(AppType appType, int appId, int subId = 0)
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