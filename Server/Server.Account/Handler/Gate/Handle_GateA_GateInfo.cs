using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.Account
{
    [MessageHandler]
    public class Handle_GateA_GateInfo : MHandler<Msg_GateA_GateInfo>
    {
        public override Task Run(Session session, Msg_GateA_GateInfo message)
        {
            GateInfo gateInfo = new GateInfo()
            {
                AppId = message.AppId,
                SubId = message.SubId,
                IP = message.IP,
                Port = message.Port,
                ClientCount = message.ClientCount,
            };
            GateInfoManager.Instance.UpdateGateInfo(gateInfo);
            return Task.CompletedTask;
        }
    }
}
