using Giant.Core;
using Giant.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Robort
{
    public class PlayerManagerComponent : InitSystem, IUpdateSystem
    {
        private static string accountPre = "wuu";
        private Queue<Player> wattingLoginList = new Queue<Player>();

        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        private Dictionary<Session, Player> playerSessions = new Dictionary<Session, Player>();

        public int TotalCount { get; set; }
        public Stopwatch Stopwatch { get; private set; }


        public static PlayerManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public void CreatePlayers(int count)
        {
            TotalCount = count;
            for (int i = 0; i < count; ++i)
            {
                wattingLoginList.Enqueue(ComponentFactory.CreateComponent<Player, string>(accountPre + i));
            }

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            while (wattingLoginList.TryDequeue(out var player))
            {
                player.DoLogin();
            }
        }

        public void AddPlayer(Player player)
        {
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

        public void EnterWorld(Player player)
        {
            playerList[player.Uid] = player;
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
