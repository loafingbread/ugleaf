namespace GameLogic.Actions.Effects;

using GameLogic.Targeting;

public class StatusEffectResult : IEffectResult
{
    public IEffectVariant Source { get; }
    public ITargetable Target { get; }
    public float Value { get; }
    public float Duration { get; }

    public StatusEffectResult(
        IEffectVariant source,
        ITargetable target,
        float value,
        float duration
    )
    {
        this.Source = source;
        this.Target = target;
        this.Value = value;
        this.Duration = duration;
    }

    public void Apply()
    {
        // TODO: Apply status effect to target — requires a status effect system on ITargetable
    }
}
