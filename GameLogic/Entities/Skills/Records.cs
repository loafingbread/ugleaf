namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillTemplateRecord
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterRecord Targeter { get; init; }
    public required List<UsableRecord> Usables { get; init; } = new();
}

public record SkillOverrideRecord : ITemplateOverride<SkillTemplateRecord>
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public List<string> Tags { get; init; } = new();
    public TargeterRecord? Targeter { get; init; }
    public List<UsableRecord> Usables { get; init; } = new();
}
