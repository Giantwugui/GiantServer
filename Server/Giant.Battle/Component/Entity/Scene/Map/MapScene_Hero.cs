using Giant.Core;
using Giant.Logger;
using Giant.Util;
using System;
using System.Collections.Generic;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, Hero> heroList = new Dictionary<int, Hero>();
        public Dictionary<int, Hero> HeroList => heroList;

        protected void UpdateHero(double dt)
        { 
        }

        protected void HeroStartFighting()
        {
            foreach (var kv in heroList)
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

        protected void HeroStopFighting()
        {
            heroList.ForEach(x => x.Value.StartFighting());
        }
    }
}
