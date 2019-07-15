using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler(AppType.Gate)]
    public class Handle_Login : MRpcHandler<Msg_CG_Login, Msg_GC_Login>
    {
        public override async Task Run(Session session, Msg_CG_Login request, Msg_GC_Login response)
        {
            var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                response.Error = ErrorCode.RpcFail;
                return;
            }

            Logger.Warn($"player login {request.Account}");

            response.Error = ErrorCode.Success;
        }
    }

    [MessageHandler(AppType.Gate)]
    public class Handle_Ping : MRpcHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override async Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response)
        {
            response.Error = ErrorCode.Success;
            await Task.CompletedTask;
        }
    }
}
