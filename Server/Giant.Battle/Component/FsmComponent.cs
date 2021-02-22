using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class FsmComponent : InitSystem<Unit>, IUpdate
    {
        private Dictionary<FsmType, BaseFsm> fsmList = new Dictionary<FsmType, BaseFsm>();

        private FsmType nextFsmType;
        private object nextFsmParam;
        private BaseFsm currFsm;

        public Unit Owner { get; private set; }

        public override void Init(Unit unit)
        {
            Owner = unit;
        }

        public void Update(double dt) 
        {
            CheckNextFsmState();

            currFsm.Update((float)dt);
        }

        public void AddFsm(BaseFsm fsm)
        {
            fsmList.Add(fsm.FsmType, fsm);
        }

        public bool SetNextFsmState(FsmType nextFsmType, object param, bool restartSameFsm = false)
        {
            if (!restartSameFsm && currFsm.FsmType == nextFsmType) return false;

            if (nextFsmType == currFsm.FsmType) return true;

            if (!fsmList.TryGetValue(nextFsmType, out BaseFsm tempNextFsm) || !tempNextFsm.CanStart())
            {
                return false;
            }

            this.nextFsmType = nextFsmType;
            nextFsmParam = param;

            return true;
        }

        public void CheckNextFsmState()
        {
            if (nextFsmType == FsmType.Base) return;

            BaseFsm oldFsm = currFsm;
            oldFsm.End();

            currFsm = fsmList[nextFsmType];
            currFsm.Start(nextFsmParam);

            nextFsmParam = null;
            nextFsmType = FsmType.Base;
        }
    }
}
