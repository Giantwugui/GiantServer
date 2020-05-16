using Giant.Core;

namespace Giant.Battle
{
    public class FsmFactory
    {
        public static BaseFsm BuildFsm(Unit unit, FsmType fsmType)
        {
            switch (fsmType)
            {
                case FsmType.Idle: return ComponentFactory.CreateComponent<FsmIdle, Unit, FsmType>(unit, fsmType);
                case FsmType.Run: return ComponentFactory.CreateComponent<FsmRun, Unit, FsmType>(unit, fsmType);
                case FsmType.Skill: return ComponentFactory.CreateComponent<FsmSkill, Unit, FsmType>(unit, fsmType);
                case FsmType.Dead: return ComponentFactory.CreateComponent<FsmDead, Unit, FsmType>(unit, fsmType);
                default:
                    return ComponentFactory.CreateComponent<BaseFsm, Unit, FsmType>(unit, FsmType.Base);
            }
        }
    }
}
