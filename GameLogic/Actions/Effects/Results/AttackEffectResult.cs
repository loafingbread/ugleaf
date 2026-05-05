namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Targeting;

public class AttackEffectResult : IEffectResult, ICritCapable
{
    public IEffectVariant EffectVariant { get; }
    public IUser User { get; }
    public ITargetable Target { get; }
    public float Damage { get; }
    public bool DidCrit { get; }

    public AttackEffectResult(
        IEffectVariant effectVariant,
        IUser user,
        ITargetable target,
        float damage,
        bool didCrit
    )
    {
        this.EffectVariant = effectVariant;
        this.User = user;
        this.Target = target;
        this.Damage = damage;
        this.DidCrit = didCrit;
    }

    public void Apply()
    {
        this.Target.TakeDamage(this.Damage);
    }
}
