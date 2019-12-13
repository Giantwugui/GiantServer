using Giant.Core;
using System.Linq;

namespace Giant.Data
{
    public class AppConfigComponent : SingleDataComponent<AppConfigComponent, AppConfig>
    {
        private readonly ListMap<AppType, AppConfig> appConfigs = new ListMap<AppType, AppConfig>();

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

        public override void Load()
        {
            appConfigs.Clear();

            //Load("AppConfig");

            AppConfig config = null;
            var datas = DataComponent.Instance.GetDatas("AppConfig");
            foreach (var kv in datas)
            {
                config = new AppConfig();
                config.Bind(kv.Value);
            }

            appConfigs.Add(config.AppType, config);
        }
    }
}