using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using GameLogic.Config;
using GameLogic.Targeting;
using GameLogic.Usables;

namespace GameLobic.Usables;

public record UsableRecord
{
    public required string Id { get; init; }
    public required TargeterRecord Targeter { get; init; }
    public required List<EffectRecord> Effects { get; init; } = new();
}

public class UsableConfig
{
    public required string Id { get; init; }
    public required TargeterConfig Targeter { get; init; }
    public required List<IUsableEffect> Effects { get; init; } = new();

    [SetsRequiredMembers]
    public UsableConfig(UsableRecord record)
    {
        this.Id = record.Id;
        this.Targeter = new TargeterConfig(record.Targeter);

        foreach (EffectRecord effectRecord in record.Effects)
        {
            IUsableEffect effect = EffectConfigFactory.CreateFromRecord(effectRecord);
            this.Effects.Add(effect);
        }
    }
}

// Stores json input file
public record EffectRecord
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

public record BurnStatusEffectRecord
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
    public static IUsableEffect CreateFromRecord(EffectRecord record)
    {
        return record.Type switch
        {
            "Status" => CreateStatusEffect(record),
            _ => throw new NotSupportedException($"Effect type {record.Type} is not supported."),
        };
    }

    private static IUsableEffect CreateStatusEffect(EffectRecord record)
    {
        return record.Subtype switch
        {
            "Burn" => CreateBurn(record),
            "Poison" => CreatePoison(record),
            _ => throw new NotSupportedException(
                $"Effect subtype {record.Subtype} is not supported."
            ),
        };
    }

    private static IUsableEffect CreateBurn(EffectRecord record)
    {
        var burnRecord = record.Parameters.Deserialize<BurnStatusEffectRecord>()!;
        var burnConfig = new BurnStatusEffectConfig(
            record.Id,
            record.Type,
            record.Subtype,
            record.Variant,
            burnRecord.Duration,
            burnRecord.DamagePerTurn
        );
        return new BurnStatusEffect(burnConfig);
    }

    private static IUsableEffect CreatePoison(EffectRecord record)
    {
        var poisonConfig = record.Parameters.Deserialize<PoisonStatusEffectConfig>()!;
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

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
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
