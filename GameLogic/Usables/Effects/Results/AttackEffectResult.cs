namespace GameLogic.Usables.Effects.Results;

using GameLogic.Entities.Characters;
using GameLogic.Targeting;

public class AttackEffectResult : IEffectResult, ICritCapable
{
    public IEffectVariant Variant { get; }
    public ITargetable Target { get; }
    public float Damage { get; }
    public bool DidCrit { get; }

    public AttackEffectResult(IEffectVariant variant, ITargetable target, float damage, bool didCrit)
    {
        Variant = variant;
        Target = target;
        Damage = damage;
        DidCrit = didCrit;
    }

    public void Apply()
    {
        if (Target is Character character)
            character.Stats.ModifyResourceStat("resource_stat_health", -(int)Damage);
    }
}
