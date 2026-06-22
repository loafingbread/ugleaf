namespace GameLogic.Usables.Effects;

public record EffectTemplateData
{
    public required string Type { get; init; }
    public required string Subtype { get; init; }
    public string? Variant { get; init; }
    public float Value { get; init; }
    public float CritChance { get; init; }
    public float Duration { get; init; }
}

public record EffectTemplatePatch
{
    public string? Type { get; init; }
    public string? Subtype { get; init; }
    public string? Variant { get; init; }
    public float? Value { get; init; }
    public float? CritChance { get; init; }
    public float? Duration { get; init; }

    public EffectTemplateData ApplyTo(EffectTemplateData baseData) => baseData with
    {
        Type = Type ?? baseData.Type,
        Subtype = Subtype ?? baseData.Subtype,
        Variant = Variant ?? baseData.Variant,
        Value = Value ?? baseData.Value,
        CritChance = CritChance ?? baseData.CritChance,
        Duration = Duration ?? baseData.Duration,
    };
}
