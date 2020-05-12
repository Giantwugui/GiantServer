using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class FsmComponent : InitSystem<Unit>, IUpdate
    {
        public override void Init(Unit unit)
        {
        }

        public void Update(double dt) { }
    }
}
