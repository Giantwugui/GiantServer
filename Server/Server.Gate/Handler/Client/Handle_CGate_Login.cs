using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Framework;
using Giant.Msg;
using Giant.Net;
using Giant.Model;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    public class Handle_Ping : MHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override async Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response)
        {
            response.Error = ErrorCode.Success;
            Client client = ClientManagerComponent.Instance.GetClient(session.Id);
            client?.HeartBeat();

            await Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class Handle_GetSecretKey : MHandler<Msg_CG_Get_SecretKey, Msg_GC_Get_SecretKey>
    {
        public override async Task Run(Session session, Msg_CG_Get_SecretKey request, Msg_GC_Get_SecretKey response)
        {
            await Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class Handle_Login : MHandler<Msg_CG_Login, Msg_GC_Login>
    {
        public override Task Run(Session session, Msg_CG_Login request, Msg_GC_Login response)
        {
            ClientEnter enter = ClientManagerComponent.Instance.GetClientEntry(request.Account);
            if (enter == null || enter.Token != request.Token)
            {
                response.Error = ErrorCode.TokenOOT;
                return Task.CompletedTask;
            }

            ClientManagerComponent.Instance.RemoveClientEntry(request.Account);
            Client client = ClientManagerComponent.Instance.GetClient(session.Id);
            client?.Dispose();

            client = ComponentFactory.CreateComponent<Client, Session, string>(session, request.Account);
            ClientManagerComponent.Instance.Add(client);

            return Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class Handle_EnterWorld : MHandler<Msg_CG_EnterWorld, Msg_GC_EnterWorld>
    {
        public override async Task Run(Session session, Msg_CG_EnterWorld request, Msg_GC_EnterWorld response)
        {
            Client client = ClientManagerComponent.Instance.GetClient(session.Id);
            if (client == null)
            {
                response.Error = ErrorCode.Fail;
                return;
            }

            var query = new MongoDBQuery<PlayerInfo>(DBName.Player, x => x.Uid == request.Uid);
            PlayerInfo playerInfo = await query.Task();
            if (playerInfo == null)
            {
                response.Error = ErrorCode.HaveNotFindCharacer;
                return;
            }

            //通知manager 负载均衡一个zone
            Msg_GateM_BalanceZone msg = new Msg_GateM_BalanceZone() { MapId = playerInfo.MapId };
            Msg_MGate_BalanceZone zone = (await NetProxyComponent.Instance.FrontendManagerServer.Call(msg)) as Msg_MGate_BalanceZone;
            FrontendComponent server = NetProxyComponent.Instance.GetFrontend(AppType.Zone, zone.ZoneId, zone.SubId);
            client.AddComponent(server);

            client.Uid = request.Uid;

            client.EnterWorld();

            response.Error = ErrorCode.Success;
        }
    }
}
