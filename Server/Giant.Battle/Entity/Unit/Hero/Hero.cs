using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Hero : Unit, IInitSystem<UnitInfo, IBattleMsgSource, IBattleMsgListener>
    {
        public override void Init(UnitInfo info, IBattleMsgSource source, IBattleMsgListener listener)
        {
            base.Init(info, source, listener);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override void UpdateInBattle(double dt)
        {
            base.UpdateInBattle(dt);
        }
    }
}
