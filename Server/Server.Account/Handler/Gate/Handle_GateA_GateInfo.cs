using Giant.Core;
using Giant.Msg;
using Giant.Net;
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
                Address = message.Address,
                ClientCount = message.ClientCount,
            };
            GateInfoComponent.Instance.UpdateGateInfo(gateInfo);
            return Task.CompletedTask;
        }
    }
}
