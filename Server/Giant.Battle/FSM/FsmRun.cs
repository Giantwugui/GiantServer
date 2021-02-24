using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Battle
{
    public class FsmRun : BaseFsm
    {
        protected override void OnStart(object param)
        {
            Owner.MoveStart();
        }

        protected override void OnUpdate(float dt)
        {
            if (Owner.CheckDestination())
            {
                Owner.MoveStart();
            }

            if (Owner.Move(dt))
            {
                IsEnd = true;
            }
        }

        protected override void OnEnd()
        {
            Owner.MoveStop();
        }
    }
}
