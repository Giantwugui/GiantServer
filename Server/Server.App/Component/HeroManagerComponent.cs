using Giant.Core;
using System.Collections.Generic;

namespace Server.App
{
    public class HeroManagerComponent : Component
    {
        private Dictionary<int, Hero> heroList = new Dictionary<int, Hero>();

        public Hero GetHero(int heroId)
        {
            heroList.TryGetValue(heroId, out Hero hero);
            return hero;
        }

    }
}
