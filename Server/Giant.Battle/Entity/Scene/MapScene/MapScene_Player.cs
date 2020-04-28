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

        public virtual void OnPlayerLeave(PlayerUnit player)
        {
        }

        public virtual void OnPlayerEnter(PlayerUnit player)
        { 
        }
    }
}
