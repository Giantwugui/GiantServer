using Giant.Core;
using Giant.Net;
using System.Collections.Generic;

namespace Robot
{
    public class PlayerManagerComponent : InitSystem, IUpdate
    {
        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        private Dictionary<long, Player> playerSessions = new Dictionary<long, Player>();

        public static PlayerManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public void AddPlayer(Player player)
        {
            playerList[player.Uid] = player;
            playerSessions[player.Session.InstanceId] = player;
        }

        public void RemovePlayer(Player player)
        {
            playerList.Remove(player.Uid);
            playerSessions.Remove(player.Session.InstanceId);
        }

        public Player GetPlayer(long sessionInstanceId)
        {
            playerSessions.TryGetValue(sessionInstanceId, out var player);
            return player;
        }

        public Player GetPlayer(int uid)
        {
            playerList.TryGetValue(uid, out var player);
            return player;
        }

        public int PlayerCount() => playerList.Count;

        public void Update(double dt)
        {
            foreach (var player in playerList)
            {
                player.Value.Update();
            }
        }

    }
}
