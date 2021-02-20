using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Framework;
using Giant.Model;
using Giant.Msg;
using Giant.Net;
using Giant.Util;
using System;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    public class Handle_Ping : MHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override async Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response, Action apply)
        {
            response.Error = ErrorCode.Success;
            Client client = ClientManagerComponent.Instance.GetClient(session.Id);
            client?.HeartBeat();
            apply();

            await Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class Handle_GetSecretKey : MHandler<Msg_CG_Get_SecretKey, Msg_GC_Get_SecretKey>
    {
        public override async Task Run(Session session, Msg_CG_Get_SecretKey request, Msg_GC_Get_SecretKey response, Action apply)
        {
            string encryptData = RSAHelper.Encrypt(session.SecretKey);
            response.Error = ErrorCode.Success;
            response.SecretKey = encryptData;
            apply();

            session.IsNeedEncrypt = true;

            await Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class Handle_Login : MHandler<Msg_CG_Login, Msg_GC_Login>
    {
        public override async Task Run(Session session, Msg_CG_Login request, Msg_GC_Login response, Action apply)
        {
            ClientEnter enter = ClientManagerComponent.Instance.GetClientEntry(request.Account);
            if (enter == null)
            {
                response.Error = ErrorCode.TokenOOT;
                apply();
                return;
            }

            ClientManagerComponent.Instance.RemoveClientEntry(request.Account);
            Client client = ClientManagerComponent.Instance.GetClient(session.Id);
            client?.Dispose();

            client = ComponentFactory.CreateComponent<Client, Session, string>(session, request.Account);
            ClientManagerComponent.Instance.Add(client);

            var query = new MongoDBQuery<PlayerInfo>(DBName.Player, x => x.Uid == request.Uid);
            PlayerInfo playerInfo = await query.Task();
            if (playerInfo == null)
            {
                response.Error = ErrorCode.HaveNotFindCharacer;
                apply();
                return;
            }

            //通知manager 负载均衡一个zone
            Msg_GateM_BalanceZone msg = new Msg_GateM_BalanceZone() { MapId = playerInfo.MapId };
            Msg_MGate_BalanceZone zone = (await NetProxyComponent.Instance.FrontendManagerServer.Call(msg)) as Msg_MGate_BalanceZone;
            FrontendComponent server = NetProxyComponent.Instance.GetFrontend(AppType.Zone, zone.ZoneId, zone.SubId);
            client.AddComponent(server);

            client.Uid = request.Uid;

            client.LoginToZone();

            response.Error = ErrorCode.Success;
            apply();
        }
    }

    
}
