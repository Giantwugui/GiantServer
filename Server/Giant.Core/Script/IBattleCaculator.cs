namespace Giant.Core
{
    public interface IBattleCalculator : IBaseScript
    {
        DamageInfo CalculateDamage(IUnit caster, IUnit target, SkillEffect effect);
    }
}
