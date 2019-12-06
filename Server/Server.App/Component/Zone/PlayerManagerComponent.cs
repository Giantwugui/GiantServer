using System;
using System.Collections.Generic;
using Giant.Core;

namespace Server.App
{
    class PlayerManagerComponent : InitSystem, IUpdateSystem
    {
        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        private Dictionary<int, Player> offlinePlayerList = new Dictionary<int, Player>();

        public static PlayerManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public void Update(double dt)
        { 
        }

        public Player GetPlayer(int uid)
        {
            playerList.TryGetValue(uid, out var player);
            return player;
        }

        public Player GetOfflinePlayer(int uid)
        {
            offlinePlayerList.TryGetValue(uid, out var player);
            return player;
        }
    }
}
