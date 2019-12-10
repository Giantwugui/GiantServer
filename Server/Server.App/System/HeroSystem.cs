using System;

namespace Server.App
{
    public class HeroSystem : IHeroSystem
    {
        public void HeroBreak(Player player, int heroId)
        {
            Hero hero = player.GetComponent<HeroManagerComponent>().GetHero(heroId);
            hero?.LevelUp(1);
        }

        public void HeroLevelUp(Player player, int heroId)
        {
            Hero hero = player.GetComponent<HeroManagerComponent>().GetHero(heroId);
            hero?.BreakUp();
        }
    }
}
