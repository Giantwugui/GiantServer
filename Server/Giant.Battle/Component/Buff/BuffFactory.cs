using Giant.EnumUtil;
using Giant.Model;
using Giant.Core;

namespace Giant.Battle
{
    public class BuffFactory
    {
        public static BaseBuff BuildBuff(Unit owner, BuffModel model)
        {
            switch ((BuffType)model.BuffType)
            {
                case BuffType.AddAttack:
                    return Scene.Pool.AddComponent<AddAttackBuff, Unit, BuffModel>(owner, model);
                default:
                    return Scene.Pool.AddComponent<DefaultBuff, Unit, BuffModel>(owner, model);
            }
        }
    }
}
