namespace GameLogic.Actions.Effects;

using GameLogic.Targeting;

public class AttackEffectResult : IEffectResult, ICritCapable
{
    public IEffectVariant Source { get; }
    public ITargetable Target { get; }
    public float Damage { get; }
    public bool DidCrit { get; }

    public AttackEffectResult(
        IEffectVariant source,
        ITargetable target,
        float damage,
        bool didCrit
    )
    {
        this.Source = source;
        this.Target = target;
        this.Damage = damage;
        this.DidCrit = didCrit;
    }

    public void Apply()
    {
        this.Target.TakeDamage(this.Damage);
    }
}
