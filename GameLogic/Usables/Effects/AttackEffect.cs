namespace GameLogic.Usables.Effects;

using GameLogic.Targeting;

public record AttackEffectParametersRecord
{
    public required int Damage { get; init; }
}

public class AttackEffectConfig : EffectConfig
{
    public int Damage { get; init; }

    public AttackEffectConfig(string id, string type, string subtype, string variant, int damage)
        : base(id, type, subtype, variant)
    {
        this.Damage = damage;
    }
}

public class AttackEffect : Effect
{
    public AttackEffect(EffectConfig config)
        : base(config) { }

    public new EffectResult Apply(IUser user, ITargetable target)
    {
        AttackEffectConfig config = (AttackEffectConfig)this._config;
        return new EffectResult(
            this,
            user.GetEntity(),
            target.GetEntity(),
            config.Damage,
            false,
            true,
            0
        );
    }
}
