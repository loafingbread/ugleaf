namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public record EffectTemplateRef : TemplateRef<EffectTemplateData, EffectTemplatePatch> { }

public record EffectTemplateData
{
    public required string DefaultType { get; set; }
    public required string DefaultSubtype { get; set; }
    public required string DefaultVariant { get; set; }
    public required string DefaultName { get; set; }
    public required string DefaultDescription { get; set; }
    public required List<string> DefaultTags { get; set; }
    public required EffectConfigData DefaultConfig { get; set; }

    public EffectTemplateData DeepCopy()
    {
        return new EffectTemplateData()
        {
            DefaultType = this.DefaultType,
            DefaultSubtype = this.DefaultSubtype,
            DefaultVariant = this.DefaultVariant,
            DefaultName = this.DefaultName,
            DefaultDescription = this.DefaultDescription,
            DefaultTags = [.. this.DefaultTags],
            DefaultConfig = this.DefaultConfig.DeepCopy(),
        };
    }
}

public record EffectTemplatePatch
{
    public string? DefaultType { get; init; }
    public string? DefaultSubtype { get; init; }
    public string? DefaultVariant { get; init; }
    public string? DefaultName { get; init; }
    public string? DefaultDescription { get; init; }
    public List<string>? DefaultTags { get; init; }
    public EffectConfigData? DefaultConfig { get; init; }
}
