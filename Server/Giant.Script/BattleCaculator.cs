using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Script
{
    public class BattleCaculator : IBattleCalculator
    {
        public DamageInfo CalculateDamage(IUnit caster, IUnit target, SkillEffect effect)
        {
            int damage = 0;
            DamageInfo info = new DamageInfo() { SkillId = effect.SkillId };

            CalculateNatureDamage(caster.GetNatures(), target.GetNatures());

            info.Damage = damage;
            return info;
        }

        private int CalculateNatureDamage(Natures casterNatures, Natures targetNatures)
        {
            int damage = targetNatures.GetNatureValue(NatureType.Defence) - casterNatures.GetNatureValue(NatureType.Attack);

            return damage > 0 ? damage : 0;
        }
    }
}
