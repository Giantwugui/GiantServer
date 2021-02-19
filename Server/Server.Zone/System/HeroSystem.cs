using Giant.Core;
using Giant.Msg;
using Giant.Net;

namespace Server.Zone
{
    [System]
    public class HeroSystem : IHeroSystem
    {
        public void HeroBreak(Player player, int heroId)
        {
            Hero hero = player.GetComponent<HeroManagerComponent>().GetHero(heroId);
            HeroSystem system = Scene.EventSystem.GetSystem<HeroSystem>();

            Msg_ZGate_Hero_Break msg = new Msg_ZGate_Hero_Break();
            if (hero == null)
            {
                msg.Error = ErrorCode.Fail;
                return;
            }
        }

        public void HeroLevelUp(Player player, int heroId)
        {
            //Hero hero = player.GetComponent<HeroManagerComponent>().GetHero(heroId);
            //hero?.BreakUp();
        }
    }
}
