namespace GameLogic.Usables.Effects;

public record BurnStatusEffectParametersRecord
{
    public required int Duration { get; init; }
    public required int DamagePerTurn { get; init; }
}

public class BurnStatusEffectConfig : StatusEffectConfigBase
{
    public int DamagePerTurn { get; init; }

    public BurnStatusEffectConfig(
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

public class BurnStatusEffect : StatusEffect
{
    public BurnStatusEffect(IEffectConfig config)
        : base(config) { }
}