using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<long, NPC> npcList = new Dictionary<long, NPC>();
        public Dictionary<long, NPC> NpcList => npcList;

        protected virtual void UpdateNpc(double dt)
        {
        }
    }
}
