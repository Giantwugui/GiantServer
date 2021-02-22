using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using Giant.EnumUtil;
using Giant.Framework;
using Giant.Model;
using Giant.Msg;
using Giant.Net;
using System;
using System.Threading.Tasks;

namespace Server.Gate
{
    [MessageHandler]
    class Handle_CGate_GetCharacter : MHandler<Msg_CG_GetCharacter, Msg_GC_GetCharacter>
    {
        public override async Task Run(Session session, Msg_CG_GetCharacter request, Msg_GC_GetCharacter response, Action reply)
        {
            ClientEnter client = ClientManagerComponent.Instance.GetClientEntry(request.Account);
            if (client == null || client.Token!= request.Token)
            {
                response.Error = ErrorCode.Fail;
                reply();
                return;
            }

            var query = new MongoDBQueryBatch<PlayerInfo>(DBName.Player, x => x.Account == client.Account);
            var playerInfos = await query.Task();

            playerInfos.ForEach(player =>
            {
                client.AddCharacter(player);
                response.Characters.Add(new Msg_CharacterInfo() { Uid = player.Uid, RoleId = player.RoleId });
            });
            response.Error = ErrorCode.Success;
            reply();
        }
    }

    [MessageHandler]
    class Handle_CGate_CreateCharacter : MHandler<Msg_CG_CreateCharacter, Msg_GC_CreateCharacter>
    {
        public override async Task Run(Session session, Msg_CG_CreateCharacter request, Msg_GC_CreateCharacter response, Action reply)
        {
            ClientEnter client = ClientManagerComponent.Instance.GetClientEntry(request.Account);
            if (client == null)
            {
                response.Error = ErrorCode.Fail;
                reply();
                return;
            }

            FrontendComponent frontendComponent = NetProxyComponent.Instance.GetFrontendSinglePoint(AppType.Manager, Scene.AppConfig.AppId);

            Msg_GateM_GetUid msg = new Msg_GateM_GetUid();
            Msg_MGate_GetUid result = (await frontendComponent.Session.Call(msg)) as Msg_MGate_GetUid;
            if (result.Error != ErrorCode.Success)
            {
                response.Error = ErrorCode.Fail;
                reply();
                return;
            }

            PlayerInfo player = await CreateCharacter(response, client.Account, result.Uid, request.RoleId);
            response.Character = new Msg_CharacterInfo() { Uid = player.Uid, RoleId = request.RoleId };

            client.AddCharacter(player);

            response.Error = ErrorCode.Success;
            reply();
        }

        private async Task<PlayerInfo> CreateCharacter(Msg_GC_CreateCharacter response, string account, int uid, int heroId)
        {
            PlayerInfo player = new PlayerInfo()
            {
                Account = account,
                Uid = uid,
                RoleId = heroId
            };

            var query = new MongoDBInsert<PlayerInfo>(DBName.Player, player);
            await query.Task();
            return player;
        }
    }
}
