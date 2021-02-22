using System.Collections.Generic;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, NPC> npcList = new Dictionary<int, NPC>();
        public Dictionary<int, NPC> NpcList => npcList;

        protected void UpdateNpc(double dt)
        {
        }
    }
}
