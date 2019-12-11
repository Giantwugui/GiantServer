using Giant.Core;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler]
    public class Handle_GetUid : MHandler<Msg_GateM_GetUid, Msg_MGate_GetUid>
    {
        public override Task Run(Session session, Msg_GateM_GetUid request, Msg_MGate_GetUid response)
        {
            response.Uid = UidComponent.Instance.GetUid();
            return Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class Handle_BalanceZone : MHandler<Msg_GateM_BalanceZone, Msg_MGate_BalanceZone>
    {
        public override Task Run(Session session, Msg_GateM_BalanceZone request, Msg_MGate_BalanceZone response)
        {
            response.ZoneId = 1001;
            response.SubId = 1;

            return Task.CompletedTask;
        }
    }
}
