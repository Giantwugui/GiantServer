using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class FsmComponent : InitSystem<Unit>, IUpdate
    {
        private Dictionary<FsmType, BaseFsm> fsmList = new Dictionary<FsmType, BaseFsm>();

        public Unit Owner { get; private set; }

        public override void Init(Unit unit)
        {
            Owner = unit;
        }

        public void Update(double dt) 
        {
        }

        public void AddFsm(BaseFsm fsm)
        {
            fsmList.Add(fsm.FsmType, fsm);
        }
    }
}
