using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using GameLogic.Config;
using GameLogic.Targeting;
using GameLogic.Usables;

namespace GameLobic.Usables;

public record UsableConfig
{
    public required string Id { get; init; }
    public required TargeterConfig Targeter { get; init; }
    public required List<EffectConfig> Effects { get; init; } = new();
}

// Stores json input file
public record EffectConfig
{
    public required string Id { get; init; }
    public required string Type { get; init; }
    public required string Subtype { get; init; }
    public required string Variant { get; init; }
    public string? BaseEffectId { get; init; }
    public required JsonElement Parameters { get; init; } // raw json node
}

public interface IEffectConfig { }

public abstract class EffectConfigBase : IEffectConfig
{
    public string Id { get; init; }
    public string Type { get; init; }
    public string Subtype { get; init; }
    public string Variant { get; init; }

    protected EffectConfigBase(string id, string type, string subtype, string variant)
    {
        this.Id = id;
        this.Type = type;
        this.Subtype = subtype;
        this.Variant = variant;
    }
}

public abstract class StatusEffectConfigBase : EffectConfigBase
{
    public int Duration { get; init; }

    public StatusEffectConfigBase(
        string id,
        string type,
        string subtype,
        string variant,
        int duration
    )
        : base(id, type, subtype, variant)
    {
        this.Duration = duration;
    }
}

class BurnStatusEffectConfig : StatusEffectConfigBase
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

class PoisonStatusEffectConfig : StatusEffectConfigBase
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

public class AttackEffectConfig : EffectConfigBase
{
    public int Damage { get; init; }

    public AttackEffectConfig(string id, string type, string subtype, string variant, int damage)
        : base(id, type, subtype, variant)
    {
        this.Damage = damage;
    }
}

public class HealEffectConfig : EffectConfigBase
{
    public int Value { get; init; }

    public HealEffectConfig(string id, string type, string subtype, string variant, int value)
        : base(id, type, subtype, variant)
    {
        this.Value = value;
    }
}

public enum EEffectType
{
    None,
    Attack,
    Heal,
    Buff,
    Debuff,
    Status,
}

public static class EffectConfigFactory
{
    public static IUsableEffect CreateFromConfig(EffectConfig config)
    {
        return config.Type switch
        {
            "Burn" => CreateBurn(config),
            "Poison" => CreatePoison(config),
            _ => throw new NotSupportedException($"Effect type {config.Type} is not supported."),
        };
    }

    private static IUsableEffect CreateBurn(EffectConfig effectConfig)
    {
        var burnConfig = effectConfig.Parameters.Deserialize<BurnStatusEffectConfig>()!;
        return new BurnStatusEffect(burnConfig);
    }

    private static IUsableEffect CreatePoison(EffectConfig effectConfig)
    {
        var poisonConfig = effectConfig.Parameters.Deserialize<PoisonStatusEffectConfig>()!;
        return new PoisonStatusEffect(poisonConfig);
    }
}

public abstract class StatusEffect : IUsableEffect
{
    public IEffectConfig Config { get; }

    protected StatusEffect(IEffectConfig config)
    {
        this.Config = config;
    }

    public UsableResult Apply(IUser user, ITargetable target)
    {
        return null;
    }
}

public class BurnStatusEffect : StatusEffect
{
    public BurnStatusEffect(IEffectConfig config)
        : base(config) { }
}

public class PoisonStatusEffect : StatusEffect
{
    public PoisonStatusEffect(IEffectConfig config)
        : base(config) { }
}
