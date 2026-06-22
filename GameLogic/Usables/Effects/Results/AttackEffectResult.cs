namespace GameLogic.Usables.Effects.Results;

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
        // TODO: Target.TakeDamage(Damage) — requires TakeDamage on ITargetable
    }
}
