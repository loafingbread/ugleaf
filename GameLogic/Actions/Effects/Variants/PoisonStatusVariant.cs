namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Entities.Stats.Query;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Targeting;

public class PoisonStatusVariant : IStatusEffectVariant
{
    private readonly EffectConfigData _config;

    public float Value => _config.Value ?? 0f;
    public float Duration => _config.Duration ?? 0f;

    public PoisonStatusVariant(EffectConfigData config)
    {
        _config = config;
    }

    public IEffectResult Compute(IUser user, ITargetable target)
    {
        float value = ResolveValue(user);
        float duration = ResolveDuration(user);
        return new StatusEffectResult(this, target, value, duration);
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

    private float ResolveDuration(IUser user)
    {
        if (_config.DurationFormula != null && user is IStatProvider sp)
        {
            return FormulaEvaluator.Evaluate(
                _config.DurationFormula,
                (id, byBase) => byBase ? sp.GetBaseValue(id!) : sp.GetCurrentValue(id!)
            );
        }
        return _config.Duration ?? 0f;
    }
}
