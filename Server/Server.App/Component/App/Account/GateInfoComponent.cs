using Giant.Core;
using Giant.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Server.App
{
    public class GateInfoComponent : Component
    {
        readonly DepthMap<int, int, GateInfo> gateInfos = new DepthMap<int, int, GateInfo>();

        public static GateInfoComponent Instance { get; } = new GateInfoComponent();

        public void UpdateGateInfo(GateInfo info)
        {
            if (gateInfos.TryGetValue(info.AppId, info.SubId, out var gateInfo))
            {
                gateInfo.Update(gateInfo);
            }
            else
            {
                gateInfo = info;
                gateInfos.Add(info.AppId, info.SubId, info);
            }

            //Log.Info($"appId {gateInfo.AppId} subId {gateInfo.SubId} client count {gateInfo.ClientCount}");
        }

        public GateInfo GetGateInfo(int appId, int subId = 1)
        {
            gateInfos.TryGetValue(appId, subId, out GateInfo info);
            return info;
        }

        public GateInfo GetGateWithBalance(int appId)
        {
            if (!gateInfos.TryGetValue(appId, out Dictionary<int, GateInfo> gates))
            {
                return null;
            }

            return gates.Values.OrderByDescending(x => x.ClientCount).FirstOrDefault();
        }
    }
}
