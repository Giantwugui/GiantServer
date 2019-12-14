using Giant.Data;

namespace Giant.Battle
{
    public class BuffFactory
    {
        public static BaseBuff BuildBuff(BuffComponent component, BuffModel model)
        {
            switch ((BuffType)model.BuffType)
            {
                case BuffType.AddAttack:
                    return component.AddComponentWithParent<AddAttackBuff>();
                default:
                    return default;
            }
        }
    }
}
