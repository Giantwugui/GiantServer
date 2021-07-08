using Giant.Core;

namespace Giant.Battle
{
    public class FsmFactory
    {
        public static BaseFsm BuildFsm(Unit unit, FsmType fsmType)
        {
            switch (fsmType)
            {
                case FsmType.Idle: return ComponentFactory.Create<FsmIdle, Unit, FsmType>(unit, fsmType);
                case FsmType.Run: return ComponentFactory.Create<FsmRun, Unit, FsmType>(unit, fsmType);
                case FsmType.Skill: return ComponentFactory.Create<FsmSkill, Unit, FsmType>(unit, fsmType);
                case FsmType.Dead: return ComponentFactory.Create<FsmDead, Unit, FsmType>(unit, fsmType);
                default:
                    return ComponentFactory.Create<BaseFsm, Unit, FsmType>(unit, FsmType.Base);
            }
        }
    }
}
