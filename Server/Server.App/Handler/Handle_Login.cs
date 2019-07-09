using Giant.DB.MongoDB;
using Giant.Log;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using System.Threading.Tasks;

namespace Server.App
{
    [MessageHandler(AppType.Gate)]
    public class Handle_Login : MRpcHandler<CR_Login, RC_Login>
    {
        public override async Task Run(Session session, CR_Login request, RC_Login response)
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
    public class Handle_Ping : MRpcHandler<CR_Ping, RC_Ping>
    {
        public override async Task Run(Session session, CR_Ping request, RC_Ping response)
        {
            response.Error = ErrorCode.Success;
        }
    }
}
