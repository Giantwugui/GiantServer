using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public class AddAttackBuff : BaseBuff, IInitSystem<BuffModel, int>
    {
        private int addValue;

        public void Init(BuffModel model, int value)
        {
            base.Init(model);

            addValue = value;
        }

        public override void Start()
        {
            owner.GetComponent<NumericalComponent>().GetNumerical(NumericalType.Attack).AddValue(addValue);
        }

        public override void End()
        {
            owner.GetComponent<NumericalComponent>().GetNumerical(NumericalType.Attack).AddValue(addValue * -1);
        }
    }
}
