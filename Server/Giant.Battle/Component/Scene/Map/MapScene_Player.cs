using Giant.Logger;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<long, PlayerUnit> playerList = new Dictionary<long, PlayerUnit>();
        public Dictionary<long, PlayerUnit> PlayerList => playerList;

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
