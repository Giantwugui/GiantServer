using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Hero
    {
        protected override void InitFsm()
        {
            base.InitFsm();
            FsmComponent.AddFsm(FsmFactory.BuildFsm(this, FsmType.Idle));
            FsmComponent.AddFsm(FsmFactory.BuildFsm(this, FsmType.Skill));
        }
    }
}
