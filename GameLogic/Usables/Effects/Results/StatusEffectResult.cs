namespace GameLogic.Usables.Effects.Results;

using GameLogic.Entities.Characters;
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
        if (Target is Character character)
            character.AddStatusEffect(new ActiveStatusEffect
            {
                VariantName = Variant.GetType().Name,
                Value = Value,
                Duration = Duration,
                RemainingTurns = Duration,
            });
    }
}
