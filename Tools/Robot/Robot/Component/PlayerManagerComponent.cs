using Giant.Core;
using Giant.Net;
using System.Collections.Generic;

namespace Robort
{
    public class PlayerManagerComponent : InitSystem, IUpdateSystem
    {
        private static string accountPre = "wuu";
        private Queue<Player> wattingLoginList = new Queue<Player>();

        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        private Dictionary<Session, Player> playerSessions = new Dictionary<Session, Player>();

        public static PlayerManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public void CreatePlayers(int num)
        {
            for (int i = 0; i < num; ++i)
            {
                wattingLoginList.Enqueue(ComponentFactory.CreateComponent<Player, string>(accountPre + i));
            }
            while (wattingLoginList.TryDequeue(out var player))
            {
                player.DoLogin();
            }
        }

        public void AddPlayer(Player player)
        {
            playerList.Add(player.Uid, player);
            playerSessions.Add(player.Session, player);
        }

        public void RemovePlayer(Player player)
        {
            playerList.Remove(player.Uid);
            playerSessions.Remove(player.Session);
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

        public void Update(double dt)
        {
            foreach (var player in playerList)
            {
                player.Value.Update();
            }
        }

    }
}
