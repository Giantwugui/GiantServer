using Giant.Core;
using Giant.Logger;
using Giant.Util;
using System;
using System.Collections.Generic;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, PlayerUnit> playerList = new Dictionary<int, PlayerUnit>();
        public Dictionary<int, PlayerUnit> PlayerList => playerList;

        protected void UpdatePlayer(double dt)
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

        public virtual void OnPlayerEnter(PlayerUnit player)
        {
        }

        public virtual void OnPlayerLeave(PlayerUnit player)
        {
        }
    }
}
