namespace GameLogic.Usables.Effects;

public record EffectTemplateRecord
{
    public required string TemplateId { get; init; }
    public required string Type { get; init; }
    public required string Subtype { get; init; }
    public required string Variant { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public List<string> Tags { get; init; } = new();
    public required EffectConfigRecord Config { get; init; }
}

public record EffectRecord : EffectTemplateRecord
{
    public string InstanceId { get; init; } = "";
}

public record EffectConfigRecord
{
    public int Value { get; init; } = 0;
    public int Duration { get; init; } = 0;
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
