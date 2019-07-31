using Giant.Share;
using System;
using System.Collections.Generic;

namespace Giant.Data
{
    public class NetTopologyConfig
    {
        private static readonly ListMap<AppType, AppConfig> needConnList = new ListMap<AppType, AppConfig>();
        private static readonly ListMap<AppType, AppConfig> needAcceptList = new ListMap<AppType, AppConfig>();

        public static void Init()
        {
            var datas = DataManager.Instance.GetDatas("NetTopology");
            InitTopology(datas);
        }


        public static List<AppConfig> GetTopology(AppType appType)
        {
            needConnList.TryGetValue(appType, out var configs);
            return configs;
        }

        private static void InitTopology(Dictionary<int, Data> topology)
        {
            Data data;
            string allApp = AppType.AllServer.ToString();

            foreach (var kv in topology)
            {
                data = kv.Value;
                AppType appType = EnumHelper.FromString<AppType>(data.GetString("Type"));

                foreach (string v in Enum.GetNames(typeof(AppType)))
                {
                    if (v == allApp || !data.GetBool(v))
                    {
                        continue;
                    }

                    BuidTopology(appType, v);
                }
            }
        }

        private static void BuidTopology(AppType source, string otherStr)
        {
            AppType other = EnumHelper.FromString<AppType>(otherStr);
            var topology = AppConfigLibrary.GetNetConfig(other);
            topology?.ForEach(x => needConnList.Add(source, x));
        }
    }
}
