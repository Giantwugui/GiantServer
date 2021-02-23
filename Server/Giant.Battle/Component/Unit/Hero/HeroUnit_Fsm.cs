using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class HeroUnit
    {
        protected override void InitFsm()
        {
            base.InitFsm();
            FsmComponent.AddFsm(FsmFactory.BuildFsm(this, FsmType.Idle));
            FsmComponent.AddFsm(FsmFactory.BuildFsm(this, FsmType.Skill));
        }
    }
}
