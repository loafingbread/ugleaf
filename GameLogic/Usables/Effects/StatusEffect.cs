namespace GameLogic.Usables.Effects;

using GameLogic.Targeting;

public abstract class StatusEffect : IEffect
{
    private EffectConfig _config { get; set; }

    protected StatusEffect(EffectConfig config)
    {
        this._config = config;
    }

    public void ApplyConfig(EffectConfig config)
    {
        this._config = config;
    }

    public EffectConfig GetConfig() => this._config;

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
    }
}
