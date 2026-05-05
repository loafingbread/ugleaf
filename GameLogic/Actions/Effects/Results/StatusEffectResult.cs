namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Targeting;

public class StatusEffectResult : IEffectResult
{
    public IEffectVariant EffectVariant { get; }
    public IUser User { get; }
    public ITargetable Target { get; }
    public float Value { get; }
    public float Duration { get; }

    public StatusEffectResult(
        IEffectVariant effectVariant,
        IUser user,
        ITargetable target,
        float value,
        float duration
    )
    {
        this.EffectVariant = effectVariant;
        this.User = user;
        this.Target = target;
        this.Value = value;
        this.Duration = duration;
    }

    public void Apply()
    {
        // TODO: Apply status effect to target — requires a status effect system on ITargetable
    }
}
