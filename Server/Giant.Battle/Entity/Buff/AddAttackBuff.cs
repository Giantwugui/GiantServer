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

        protected override void OnStart()
        {
            owner.GetComponent<NumericalComponent>().GetNumerical(NumericalType.Attack).AddValue(addValue);
        }

        protected override void OnEnd()
        {
            owner.GetComponent<NumericalComponent>().GetNumerical(NumericalType.Attack).AddValue(addValue * -1);
        }
    }
}
