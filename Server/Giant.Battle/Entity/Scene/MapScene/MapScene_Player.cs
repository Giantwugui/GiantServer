using Giant.Core;
using Giant.Util;
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
            playerList.ForEach(x => x.Value.StartFighting());
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
