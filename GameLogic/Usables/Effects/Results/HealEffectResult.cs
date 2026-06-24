namespace GameLogic.Usables.Effects.Results;

using GameLogic.Entities.Characters;
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
        if (Target is Character character)
            character.Stats.ModifyResourceStat("resource_stat_health", (int)Amount);
    }
}
