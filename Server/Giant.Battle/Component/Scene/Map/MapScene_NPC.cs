using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, NPC> npcList = new Dictionary<int, NPC>();
        public Dictionary<int, NPC> NpcList => npcList;

        public List<NPC> GetNPCs()
        {
            return npcList.Values.ToList();
        }

        protected virtual void UpdateNpc(double dt)
        {
        }
    }
}
