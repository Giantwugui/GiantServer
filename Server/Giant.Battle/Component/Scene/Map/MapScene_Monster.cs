using Giant.Logger;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<long, Monster> monsterList = new Dictionary<long, Monster>();
        public Dictionary<long, Monster> MonsterList => monsterList;


        public void AddMonster(Monster monster)
        { 
        }


        protected virtual void UpdateMonster(double dt)
        {
        }

        protected void MonsterStartFighting()
        {
            foreach (var kv in monsterList)
            {
                try
                {
                    kv.Value.StartFighting();
                }
                catch (Exception ex)
                {
                    Log.Error($"hero {kv.Key} start fighting error {ex}");
                }
            }
        }

        protected void MonsterStopFighting()
        {
            monsterList.ForEach(x => x.Value.StartFighting());
        }
    }
}
