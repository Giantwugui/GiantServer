using Giant.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class PlayerManager
    {
        private Dictionary<Session, Player> playerSessions = new Dictionary<Session, Player>();
        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();

        public static PlayerManager Instance { get; } = new PlayerManager();

        public void AddPlayer(Player player)
        {
            playerList.Add(player.Uid, player);
            playerSessions.Add(player.Session, player);
        }

        public Player GetPlayer(Session session)
        {
            playerSessions.TryGetValue(session, out var player);
            return player;
        }

        public Player GetPlayer(int uid)
        {
            playerList.TryGetValue(uid, out var player);
            return player;
        }

        public void Update()
        {
            foreach (var player in playerList)
            {
                player.Value.Update();
            }
        }

    }
}
