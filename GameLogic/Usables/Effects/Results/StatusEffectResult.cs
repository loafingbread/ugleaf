namespace GameLogic.Usables.Effects.Results;

using GameLogic.Targeting;

public class StatusEffectResult : IEffectResult
{
    public IEffectVariant Variant { get; }
    public ITargetable Target { get; }
    public float Value { get; }
    public float Duration { get; }

    public StatusEffectResult(IEffectVariant variant, ITargetable target, float value, float duration)
    {
        Variant = variant;
        Target = target;
        Value = value;
        Duration = duration;
    }

    public void Apply()
    {
        // TODO: Target.ApplyStatusEffect(...) — requires status effect system on ITargetable
    }
}
