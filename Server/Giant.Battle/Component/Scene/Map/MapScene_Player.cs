using Giant.Logger;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, PlayerUnit> playerList = new Dictionary<int, PlayerUnit>();
        public Dictionary<int, PlayerUnit> PlayerList => playerList;

        public List<PlayerUnit> GetPlayers()
        {
            return playerList.Values.ToList();
        }

        protected virtual void UpdatePlayer(double dt)
        {
        }

        protected void PlayerStartFighting()
        {
            foreach (var kv in playerList)
            {
                try
                {
                    kv.Value.StartFighting();
                }
                catch (Exception ex)
                {
                    Log.Error($"player {kv.Key} start fighting error {ex}");
                }
            }
        }

        protected void PlayerStopFighting()
        {
            playerList.ForEach(x => x.Value.StartFighting());
        }
    }
}
