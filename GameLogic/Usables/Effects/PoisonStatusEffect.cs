namespace GameLogic.Usables.Effects;

public class PoisonStatusEffectConfig : StatusEffectConfigBase
{
    public int DamagePerTurn { get; init; }

    public PoisonStatusEffectConfig(
        string id,
        string type,
        string subtype,
        string variant,
        int duration,
        int damagePerTurn
    )
        : base(id, type, subtype, variant, duration)
    {
        this.DamagePerTurn = damagePerTurn;
    }
}

public class PoisonStatusEffect : StatusEffect
{
    public PoisonStatusEffect(IEffectConfig config)
        : base(config) { }
}
