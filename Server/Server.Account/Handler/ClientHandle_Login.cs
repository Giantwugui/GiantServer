using System.Threading.Tasks;
using Giant.Msg;
using Giant.Net;
using Giant.Share;
using Giant.Log;
using Giant.DB.MongoDB;

namespace Server.Account
{
    [MessageHandler(AppType.Account)]
    class ClientHandle_Login : RpcMHandler<Msg_CA_Login, Msg_AC_Login>
    {
        public override async Task Run(Session session, Msg_CA_Login request, Msg_AC_Login response)
        {
            var query = new MongoDBQuery<AccountInfo>("Account", x => x.Account == request.Account);
            AccountInfo account = await query.Task();
            if (account == null)
            {
                account = new AccountInfo() { Account = request.Account, Password = request.Password };
                var insertQuery = new MongoDBInsert<AccountInfo>("Account", account);
                await insertQuery.Task();
            }

            //负载均衡一个gate
            response.Address = "127.0.0.1";
            response.Error = ErrorCode.Success;

            Logger.Debug($"user login {request.Account}");
        }
    }
}
