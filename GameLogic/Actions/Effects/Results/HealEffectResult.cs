namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Targeting;

public class HealEffectResult : IEffectResult
{
    public IEffectVariant EffectVariant { get; }
    public IUser User { get; }
    public ITargetable Target { get; }
    public float Amount { get; }

    public HealEffectResult(IEffectVariant effectVariant, IUser user, ITargetable target, float amount)
    {
        this.EffectVariant = effectVariant;
        this.User = user;
        this.Target = target;
        this.Amount = amount;
    }

    public void Apply()
    {
        // TODO: Target.Heal(Amount) — requires Heal() on ITargetable
    }
}
