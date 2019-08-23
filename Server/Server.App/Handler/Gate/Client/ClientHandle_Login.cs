using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler(AppType.Gate)]
    public class ClientHandle_Ping : RpcMHandler<Msg_CG_HeartBeat_Ping, Msg_GC_HeartBeat_Pong>
    {
        public override async Task Run(Session session, Msg_CG_HeartBeat_Ping request, Msg_GC_HeartBeat_Pong response)
        {
            response.Error = ErrorCode.Success;
            Logger.Debug($"client heart beat !");
            await Task.CompletedTask;
        }
    }

    [MessageHandler(AppType.Gate)]
    public class ClientHandle_GetSecretKey : RpcMHandler<Msg_CG_Get_SecretKey, Msg_GC_Get_SecretKey>
    {
        public override async Task Run(Session session, Msg_CG_Get_SecretKey request, Msg_GC_Get_SecretKey response)
        {
            await Task.CompletedTask;
        }
    }

    [MessageHandler(AppType.Gate)]
    public class ClientHandle_Login : RpcMHandler<Msg_CG_Login, Msg_GC_Login>
    {
        public override async Task Run(Session session, Msg_CG_Login request, Msg_GC_Login response)
        {
            var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                account = new AccountInfo()
                {
                    Account = request.Account,
                };

                var insertQuery = new MongoDBInsert<AccountInfo>("Account", account);
                bool result = await insertQuery.Task();
            }

            Logger.Warn($"user login {request.Account}");

            response.Error = ErrorCode.Success;
        }
    }
}
