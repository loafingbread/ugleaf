namespace GameLogic.Usables.Effects;

using System.Text.Json;

// Effect json minimum format
public interface IEffectRecord
{
    public string Id { get; }
    public string Type { get; }
    public string Subtype { get; }
    public string Variant { get; }
    public string? BaseEffectId { get; }
    public JsonElement Parameters { get; } // raw json node
}


// Reads json data file
public record EffectRecord : IEffectRecord
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

public enum EEffectType
{
    None,
    Attack,
    Heal,
    Buff,
    Debuff,
    Status,
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
