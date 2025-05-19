using GameLogic.Targeting;

namespace GameLogic.Usables.Effects;

public abstract class Effect : IEffect
{
    protected EffectConfig _config { get; set; }

    protected Effect(EffectConfig config)
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
