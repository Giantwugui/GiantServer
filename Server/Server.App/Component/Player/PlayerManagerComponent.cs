using Giant.Core;
using Giant.Framework;
using System;
using System.Collections.Generic;

namespace Server.App
{
    class PlayerManagerComponent : InitSystem, IUpdateSystem
    {
        private static readonly short offlineKeepTime = 10;

        private long timerId;
        private readonly Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        private readonly Dictionary<int, Player> offlinePlayerList = new Dictionary<int, Player>();

        public static PlayerManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
            TimerComponent.Instance.AddRepeatTimer(60 * 1000, RemoveOfflinePlayer);
        }

        public void Update(double dt)
        {
            foreach (var kv in playerList)
            {
                kv.Value.Update();
            }
        }

        public void PlayerOnline(Player player)
        {
            playerList.Add(player.Uid, player);
        }

        public Player GetPlayer(int uid)
        {
            playerList.TryGetValue(uid, out var player);
            return player;
        }

        public void PlayerOffline(Player player)
        {
            playerList.Remove(player.Uid);
            offlinePlayerList[player.Uid] = player;
        }

        public Player GetOfflinePlayer(int uid)
        {
            offlinePlayerList.TryGetValue(uid, out var player);
            return player;
        }

        public override void Dispose()
        {
            base.Dispose();

            playerList.ForEach(x => x.Value.Dispose());
            offlinePlayerList.ForEach(x => x.Value.Dispose());

            playerList.Clear();
            offlinePlayerList.Clear();
            TimerComponent.Instance.Remove(timerId);

            timerId = 0;
        }

        private void RemoveOfflinePlayer()
        {
            DateTime now = TimeHelper.Now;
            foreach (var kv in offlinePlayerList)
            {
                if (kv.Value.IsOnline) continue;
                if ((now - kv.Value.OfflineTime).TotalMinutes > offlineKeepTime)
                {
                    kv.Value.Dispose();
                }
            }
            offlinePlayerList.Clear();
        }
    }
}
