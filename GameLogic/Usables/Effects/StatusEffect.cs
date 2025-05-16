namespace GameLogic.Usables.Effects;

using GameLogic.Targeting;

public abstract class StatusEffect : IEffect
{
    public IEffectConfig Config { get; }

    protected StatusEffect(IEffectConfig config)
    {
        this.Config = config;
    }

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
    }
}
