using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, Monster> monsterList = new Dictionary<int, Monster>();
        public Dictionary<int, Monster> MonsterList => monsterList;

        protected void UpdateMonster(double dt)
        {
        }

        protected void MonsterStartFighting()
        {
            monsterList.ForEach(x => x.Value.StartFighting());
        }

        protected void MonsterStopFighting()
        {
            monsterList.ForEach(x => x.Value.StartFighting());
        }
    }
}
