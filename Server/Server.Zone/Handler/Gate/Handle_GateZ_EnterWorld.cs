using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Logger;
using Giant.Model;
using Giant.Msg;
using Giant.Net;
using System.Threading.Tasks;

namespace Server.Zone
{
    [MessageHandler]
    public class Handle_GateZ_EnterWorld : MHandler<Msg_GateZ_EnterWorld>
    {
        public override async Task Run(Session session, Msg_GateZ_EnterWorld message)
        {
            Player player = PlayerManagerComponent.Instance.GetPlayer(message.Uid);
            if (player == null)
            {
                player = PlayerManagerComponent.Instance.GetOfflinePlayer(message.Uid);
            }

            if (player == null)
            {
                MongoDBQuery<PlayerInfo> query = new MongoDBQuery<PlayerInfo>(DBName.Player, x => x.Uid == message.Uid);
                PlayerInfo info = await query.Task();

                if (info == null)
                {
                    Log.Error($"Have not find player db info uid {message.Uid}");
                    return;
                }

                player = ComponentFactory.Create<Player, PlayerInfo>(info);
            }

            PlayerManagerComponent.Instance.PlayerOnline(player);
            player.EnterWorld();
        }
    }

}
