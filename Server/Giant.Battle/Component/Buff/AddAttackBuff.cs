using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;

namespace Giant.Battle
{
    public class AddAttackBuff : BaseBuff
    {
        private int addValue;

        protected override void OnInit()
        {
            addValue = 0;
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
