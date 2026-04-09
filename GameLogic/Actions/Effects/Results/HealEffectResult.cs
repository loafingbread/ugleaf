namespace GameLogic.Actions.Effects;

using GameLogic.Targeting;

public class HealEffectResult : IEffectResult
{
    public IEffectVariant Source { get; }
    public ITargetable Target { get; }
    public float Amount { get; }

    public HealEffectResult(IEffectVariant source, ITargetable target, float amount)
    {
        this.Source = source;
        this.Target = target;
        this.Amount = amount;
    }

    public void Apply()
    {
        // TODO: Target.Heal(Amount) — requires Heal() on ITargetable
    }
}
