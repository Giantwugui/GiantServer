using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Msg;
using Giant.Net;
using Server.Frame;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    class Handle_CGate_CreateCharacter : RpcMHandler<Msg_CG_CreateCharacter, Msg_GC_CreateCharacter>
    {
        public override async Task Run(Session session, Msg_CG_CreateCharacter request, Msg_GC_CreateCharacter response)
        {
            var query = new MongoDBQuery<PlayerInfo>(DBName.Player, x => x.Uid == request.Uid);
            PlayerInfo playerInfo = await query.Task();
            if (playerInfo != null)
            {
                response.Error = ErrorCode.HadExistTheCharacter;
                return;
            }

            PlayerInfo player = await CreateCharacter(response, request.Uid, request.HeroId);

            response.Error = ErrorCode.Success;
        }

        private async Task<PlayerInfo> CreateCharacter(Msg_GC_CreateCharacter response, int uid, int heroId)
        {
            PlayerInfo player = new PlayerInfo()
            {
                Uid = uid,
                HeroId = heroId
            };

            var query = new MongoDBInsert<PlayerInfo>(DBName.Player, player);
            await query.Task();
            return player;
        }
    }
}
