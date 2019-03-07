using Giant.Model;

namespace Giant.Hotfix
{
    [ObjectSystem]
    class PlayerUpdateSystem : UpdateSystem<Player>
    {
        public override void Update(Player self)
        {
            self.Update();
        }
    }


    static class PlayerHelper
    {
        public static void Update(this Player player)
        {
        }
    }
}
