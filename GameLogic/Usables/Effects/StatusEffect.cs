namespace GameLogic.Usables.Effects;

using GameLogic.Targeting;

public abstract class StatusEffect : IEffect
{
    private IEffectConfig _config { get; set; }

    protected StatusEffect(IEffectConfig config)
    {
        this._config = config;
    }

    public void ApplyConfig(IEffectConfig config)
    {
        this._config = config;
    }

    public IEffectConfig GetConfig() => this._config;

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
    }
}
