using Giant.Core;

namespace Server.Zone
{
    public interface IHeroSystem : ISystem
    {
        void HeroLevelUp(Player player, int heroId);
        void HeroBreak(Player player, int heroId);
    }
}
