namespace GameLogic.Actions.Effects;

using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

public record EffectTemplateRef : TemplateRef<EffectTemplateData, EffectTemplatePatch> { }

/// <summary>
/// Flat data record for an effect. Used directly as both the JSON-deserialized
/// template content (EffectTemplateRef.TemplateValue) and the resolved data passed
/// to EffectVariantFactory. No separate Spec type is needed because effects are
/// leaf types with no load-time cross-references.
/// </summary>
public record EffectTemplateData
{
    public required string DefaultName { get; init; }
    public required string DefaultDescription { get; init; }
    public required List<string> DefaultTags { get; init; }

    /// <summary>"Attack" | "Heal" | "Status"</summary>
    public required string DefaultType { get; init; }

    /// <summary>"SingleHit" | "Burn" | "Poison" etc.</summary>
    public required string DefaultSubtype { get; init; }

    public required EffectConfigData DefaultConfig { get; init; }

    public EffectTemplateData DeepCopy() =>
        new()
        {
            DefaultName = this.DefaultName,
            DefaultDescription = this.DefaultDescription,
            DefaultTags = [.. this.DefaultTags],
            DefaultType = this.DefaultType,
            DefaultSubtype = this.DefaultSubtype,
            DefaultConfig = this.DefaultConfig.DeepCopy(),
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
public record EffectTemplatePatch
{
    public string? DefaultName { get; init; }
    public string? DefaultDescription { get; init; }
    public List<string>? DefaultTags { get; init; }
    public string? DefaultType { get; init; }
    public string? DefaultSubtype { get; init; }

    /// <summary>
    /// Replaces the entire Config when set (whole-object override, same as StatPatch.Capabilities).
    /// </summary>
    public EffectConfigData? DefaultConfig { get; init; }

    public EffectTemplatePatch ApplyTo(EffectTemplateData baseData) =>
        new()
        {
            DefaultName = this.DefaultName ?? baseData.DefaultName,
            DefaultDescription = this.DefaultDescription ?? baseData.DefaultDescription,
            DefaultTags = this.DefaultTags ?? [.. baseData.DefaultTags],
            DefaultType = this.DefaultType ?? baseData.DefaultType,
            DefaultSubtype = this.DefaultSubtype ?? baseData.DefaultSubtype,
            DefaultConfig = this.DefaultConfig ?? baseData.DefaultConfig.DeepCopy(),
        };
}
