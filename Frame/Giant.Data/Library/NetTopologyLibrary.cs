using Giant.Core;
using Giant.Share;
using System;
using System.Collections.Generic;

namespace Giant.Data
{
    public enum NetTopologyType
    {
        None = 0,
        ConnectAll = 1,
        AcceptAll = 2,
        ConnectByApp = 3,
        AcceptByApp = 4,
    }

    public class NetTopologyLibrary
    {
        private static readonly DepthMap<AppType, AppType, NetTopologyType> netTopology = new DepthMap<AppType, AppType, NetTopologyType>();

        public static void Init()
        {
            var datas = DataManager.Instance.GetDatas("NetTopology");
            InitTopology(datas);
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
                    if (v == allApp)
                    {
                        continue;
                    }

                    AppType other = EnumHelper.FromString<AppType>(v);
                    NetTopologyType topologyType = EnumHelper.FromString<NetTopologyType>(data.GetString(v));
                    netTopology.Add(appType, other, topologyType);
                }
            }
        }

        public static bool NeedConnect(AppType appType, int appId, AppType otherAppType, int otherAppId)
        {
            if (!netTopology.TryGetValue(appType, otherAppType, out var topologyType))
            {
                return false;
            }
            switch (topologyType)
            {
                case NetTopologyType.ConnectAll: return true;
                case NetTopologyType.ConnectByApp: return appId == otherAppId;
                default: return false;
            }
        }

        public static bool NeeAccept(AppType appType, int appId, AppType otherAppType, int otherAppId)
        {
            if (!netTopology.TryGetValue(appType, otherAppType, out var topologyType))
            {
                return false;
            }
            switch (topologyType)
            {
                case NetTopologyType.AcceptAll: return true;
                case NetTopologyType.AcceptByApp: return appId == otherAppId;
                default: return false;
            }
        }

    }
}
