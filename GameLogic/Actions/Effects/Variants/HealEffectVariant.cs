namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Entities.Stats.Query;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Targeting;

public class HealEffectVariant : IHealEffectVariant
{
    private readonly EffectConfigData _config;

    public float Value => _config.Value ?? 0f;

    public HealEffectVariant(EffectConfigData config)
    {
        _config = config;
    }

    public IEffectResult Compute(IUser user, ITargetable target)
    {
        float amount = ResolveValue(user);
        return new HealEffectResult(this, target, amount);
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
