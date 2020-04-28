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
    }
}
