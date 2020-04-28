using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class PlayerUnit : Unit, IInitSystem<UnitInfo, IBattleMsgSource, IBattleMsgListener>, IUpdate
    {
        public override void Init(UnitInfo info, IBattleMsgSource source, IBattleMsgListener listener)
        {
            base.Init(info, source, listener);
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }
    }
}
