using Giant.Core;
using Giant.Data;

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
            BuffModel model = BuffDataComponent.Instance.GetModel(buffId);
        }

        public void Update(double dt)
        { 
        }
    }
}
