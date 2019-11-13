using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Server.Model;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    public class ClientHandle_Ping : RpcMHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override async Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response)
        {
            response.Error = ErrorCode.Success;
            Logger.Debug($"client heart beat !");
            Client client = ClientManager.Instance.GetClient(session.Id);
            client?.HeartBeat();

            await Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class ClientHandle_GetSecretKey : RpcMHandler<Msg_CG_Get_SecretKey, Msg_GC_Get_SecretKey>
    {
        public override async Task Run(Session session, Msg_CG_Get_SecretKey request, Msg_GC_Get_SecretKey response)
        {
            await Task.CompletedTask;
        }
    }

    [MessageHandler]
    public class ClientHandle_Login : RpcMHandler<Msg_CG_Login, Msg_GC_Login>
    {
        public override async Task Run(Session session, Msg_CG_Login request, Msg_GC_Login response)
        {
            ClientEnter entry = ClientManager.Instance.GetClientEntry(request.Account);
            if (entry == null)
            {
                response.Error = ErrorCode.Fail;
                return;
            }

            var query = new MongoDBQuery<PlayerInfo>("Player", x => x.Uid == request.Uid);
            PlayerInfo playerInfo = await query.Task();
            if (playerInfo == null)
            {
                response.Error = ErrorCode.HaveNotFindCharacer;
                return;
            }
        }
    }
}
