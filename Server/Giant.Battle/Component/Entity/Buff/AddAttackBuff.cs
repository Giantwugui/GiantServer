using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;

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
            owner.GetComponent<NatureComponent>().GetNature(NatureType.Attack).AddValue(addValue);
        }

        protected override void OnEnd()
        {
            owner.GetComponent<NatureComponent>().GetNature(NatureType.Attack).AddValue(addValue * -1);
        }
    }
}
