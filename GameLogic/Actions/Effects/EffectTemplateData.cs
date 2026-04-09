namespace GameLogic.Actions.Effects;

using GameLogic.Entities.Stats.Stat;

/// <summary>
/// Flat data record for an effect. Used directly as both the JSON-deserialized
/// template content (EffectTemplateRef.TemplateValue) and the resolved data passed
/// to EffectVariantFactory. No separate Spec type is needed because effects are
/// leaf types with no load-time cross-references.
/// </summary>
public record EffectData
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    /// <summary>"Attack" | "Heal" | "Status"</summary>
    public required string Type { get; init; }

    /// <summary>"SingleHit" | "Burn" | "Poison" etc.</summary>
    public required string Subtype { get; init; }

    public required EffectConfigData Config { get; init; }

    public EffectData DeepCopy() =>
        new()
        {
            Name = this.Name,
            Description = this.Description,
            Tags = [.. this.Tags],
            Type = this.Type,
            Subtype = this.Subtype,
            Config = this.Config.DeepCopy(),
        };
}

public record EffectConfigData
{
    /// <summary>
    /// Flat damage/heal/status value. Used when ValueFormula is null.
    /// </summary>
    public float? Value { get; init; }

    public float? CritChance { get; init; }

    /// <summary>
    /// Duration in turns/seconds. Used when DurationFormula is null.
    /// </summary>
    public float? Duration { get; init; }

    /// <summary>
    /// Formula evaluated at runtime against the user's IStatProvider.
    /// Supports stat references, constants, operators, and conditionals.
    /// Uses FormulaEvaluator from GameLogic/Entities/Stats/Stat/Formula/.
    /// Mutually exclusive with Value per field.
    /// </summary>
    public FormulaData? ValueFormula { get; init; }

    public FormulaData? DurationFormula { get; init; }

    public EffectConfigData DeepCopy() =>
        new()
        {
            Value = this.Value,
            CritChance = this.CritChance,
            Duration = this.Duration,
            ValueFormula = this.ValueFormula?.DeepCopy(),
            DurationFormula = this.DurationFormula?.DeepCopy(),
        };
}

/// <summary>
/// Nullable override fields applied on top of a base EffectData when using
/// the Override template kind.
/// </summary>
public record EffectPatch
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public string? Type { get; init; }
    public string? Subtype { get; init; }

    /// <summary>
    /// Replaces the entire Config when set (whole-object override, same as StatPatch.Capabilities).
    /// </summary>
    public EffectConfigData? Config { get; init; }

    public EffectData ApplyTo(EffectData baseData) =>
        new()
        {
            Name = this.Name ?? baseData.Name,
            Description = this.Description ?? baseData.Description,
            Tags = this.Tags ?? [.. baseData.Tags],
            Type = this.Type ?? baseData.Type,
            Subtype = this.Subtype ?? baseData.Subtype,
            Config = this.Config ?? baseData.Config.DeepCopy(),
        };
}
