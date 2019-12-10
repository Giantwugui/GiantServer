using Giant.Core;

namespace Server.App
{
    public interface IHeroSystem : ISystem
    {
        void HeroLevelUp(Player player, int heroId);
        void HeroBreak(Player player, int heroId);
    }
}
