using Giant.Core;

namespace Giant.Battle
{
    public class BuffComponent : InitSystem, IUpdate
    {
        private readonly ListMap<BuffType, BaseBuff> buffs = new ListMap<BuffType, BaseBuff>();

        public override void Init()
        {
        }

        public void AddBuff(int buffId)
        { 
        }

        public void Update(double dt)
        { 
        }
    }
}
