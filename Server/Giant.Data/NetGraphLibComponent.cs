using Giant.Core;
using System;
using System.Collections.Generic;

namespace Giant.Data
{
    public class NetGraphLibComponent : InitSystem, ILoadSystem
    {
        private readonly DepthMap<AppType, AppType, NetGraphType> netTopology = new DepthMap<AppType, AppType, NetGraphType>();

        public NetGraphLibComponent() { }

        public override void Init()
        {
            Load();
        }

        public void Load()
        {
            netTopology.Clear();
            var datas = DataComponent.Instance.GetDatas("NetGraph");
            InitTopology(datas);
        }

        public bool NeedConnect(AppType appType, int appId, AppType otherAppType, int otherAppId)
        {
            if (!netTopology.TryGetValue(appType, otherAppType, out var topologyType))
            {
                return false;
            }
            switch (topologyType)
            {
                case NetGraphType.ConnectAll: return true;
                case NetGraphType.ConnectByApp: return appId == otherAppId;
                default: return false;
            }
        }

        public bool NeeAccept(AppType appType, int appId, AppType otherAppType, int otherAppId)
        {
            if (!netTopology.TryGetValue(appType, otherAppType, out var topologyType))
            {
                return false;
            }
            switch (topologyType)
            {
                case NetGraphType.AcceptAll: return true;
                case NetGraphType.AcceptByApp: return appId == otherAppId;
                default: return false;
            }
        }

        private void InitTopology(Dictionary<int, DataModel> topology)
        {
            DataModel data;
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
                    NetGraphType topologyType = EnumHelper.FromString<NetGraphType>(data.GetString(v));
                    netTopology.Add(appType, other, topologyType);
                }
            }
        }

    }
}
