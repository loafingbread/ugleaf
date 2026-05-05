namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Entities.Stats.Query;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Targeting;

public class AttackEffectVariant : IAttackEffectVariant
{
    private readonly EffectConfigData _config;

    public float Value => _config.Value ?? 0f;
    public float CritChance => _config.CritChance ?? 0f;

    public AttackEffectVariant(EffectConfigData config)
    {
        _config = config;
    }

    public IEffectResult Compute(IUser user, ITargetable target)
    {
        float damage = ResolveValue(user);
        bool didCrit = Random.Shared.NextSingle() < this.CritChance;
        if (didCrit)
            damage *= 2f;

        return new AttackEffectResult(this, user, target, damage, didCrit);
    }

    private float ResolveValue(IUser user)
    {
        if (_config.ValueFormula != null && user is IStatProvider sp)
        {
            return FormulaEvaluator.Evaluate(
                _config.ValueFormula,
                (id, byBase) => byBase ? sp.GetBaseValue(id!) : sp.GetCurrentValue(id!)
            );
        }
        return _config.Value ?? 0f;
    }
}
