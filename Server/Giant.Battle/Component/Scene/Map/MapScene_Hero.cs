using Giant.Logger;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private Dictionary<int, HeroUnit> heroList = new Dictionary<int, HeroUnit>();
        public Dictionary<int, HeroUnit> HeroList => heroList;

        public List<HeroUnit> GetHeroes()
        {
            return heroList.Values.ToList();
        }
        
        protected virtual void UpdateHero(double dt)
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
