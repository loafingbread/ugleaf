namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public record CharacterTemplateRecord : IStatBlockRecord
{
    public required string Name { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();
    public required List<StatRecord> Stats { get; init; } = new();
    public required List<SkillTemplateRecord> Skills { get; init; } = new();
}

public record CharacterOverrideRecord : ITemplateOverride<CharacterTemplateRecord>
{
    public string? Name { get; init; } = "";
    public string? Description { get; init; } = "";
    public List<string>? Tags { get; init; } = new();
    public List<StatRecord>? Stats { get; init; } = new();
    public List<SkillTemplateRecord>? Skills { get; init; } = new();
}
