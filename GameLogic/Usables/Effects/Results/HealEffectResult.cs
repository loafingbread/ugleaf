namespace GameLogic.Usables.Effects.Results;

using GameLogic.Targeting;

public class HealEffectResult : IEffectResult
{
    public IEffectVariant Variant { get; }
    public ITargetable Target { get; }
    public float Amount { get; }

    public HealEffectResult(IEffectVariant variant, ITargetable target, float amount)
    {
        Variant = variant;
        Target = target;
        Amount = amount;
    }

    public void Apply()
    {
        // TODO: Target.Heal(Amount) — requires Heal on ITargetable
    }
}
