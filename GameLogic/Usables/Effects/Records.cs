namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public record EffectTemplateSpec
{
    public required string Type { get; init; }
    public required string Subtype { get; init; }
    public required string Variant { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
    public required EffectConfigData Config { get; init; }

    public EffectTemplateSpec(EffectTemplateSpec template)
    {
        this.Type = template.Type.ToString();
        this.Subtype = template.Subtype;
        this.Variant = template.Variant;
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = template.Tags;
        this.Config = template.Config;
    }
}

public record EffectOverrideSpec
{
    public string? Type { get; init; }
    public string? Subtype { get; init; }
    public string? Variant { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public EffectConfigData? Config { get; init; }
}

public record EffectInstanceSpec : EffectTemplateSpec { }

public record EffectData : EffectTemplateSpec
{
    public required ReferenceId ReferenceId { get; init; }

    public EffectData(EffectData data)
        : base(data)
    {
        this.ReferenceId = data.ReferenceId;
    }
}

public record EffectConfigData
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
