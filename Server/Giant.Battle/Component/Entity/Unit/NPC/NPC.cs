using Giant.Core;
using Giant.Model;

namespace Giant.Battle
{
    public partial class NPC : Unit, IInitSystem<Map, NPCModel>
    {
        public void Init(Map mapScene, NPCModel model)
        {
            base.Init(mapScene, UnitType.NPC);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override bool IsAny(Unit unit)
        {
            return false;
        }

        protected override bool IsEnemy(Unit unit)
        {
            return false;
        }

        protected override bool IsAutoAI()
        {
            return false;
        }
    }
}
