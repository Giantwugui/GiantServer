using Giant.Share;
using System;
using System.Collections.Generic;

namespace Giant.Data
{
    public class NetTopologyConfig
    {
        private static readonly ListMap<AppType, NetConfigModel> netTopology = new ListMap<AppType, NetConfigModel>();

        public static void Init()
        {
            var datas = DataManager.Instance.GetDatas("NetTopology");
            InitTopology(datas);
        }


        public static List<NetConfigModel> GetTopology(AppType appType)
        {
            netTopology.TryGetValue(appType, out var configs);
            return configs;
        }

        private static void InitTopology(Dictionary<int, Data> topology)
        {
            Data data;
            string allApp = AppType.AllServer.ToString();

            foreach (var kv in topology)
            {
                data = kv.Value;
                string name = data.GetString("Name");
                AppType appType = (AppType)Enum.Parse(typeof(AppType), name);


                foreach (string v in Enum.GetNames(typeof(AppType)))
                {
                    if (v == allApp)
                    {
                        continue;
                    }

                    int needConn = data.GetInt(v);
                    if (needConn == 0)
                    {
                        continue;
                    }

                    BuidTopology(appType, v);
                }
            }
        }

        private static void BuidTopology(AppType source, string otherStr)
        {
             AppType other = (AppType)Enum.Parse(typeof(AppType), otherStr);
            var topology = NetConfig.GetNetConfig(other);
            foreach (var kv in topology)
            {
                netTopology.Add(source, kv.Value);
            }
        }
    }
}
