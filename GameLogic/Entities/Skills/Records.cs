namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;

public record SkillTemplateRecord
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterRecord Targeter { get; init; }

    // TODO: Do I need to include typing enforcement for usable type here?

    public required List<ReferenceUnionSpec> Usables { get; init; } = new();
}

public record SkillOverrideRecord : ITemplateOverride<SkillTemplateRecord>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public TargeterRecord? Targeter { get; init; }
    public List<ReferenceUnionSpec>? Usables { get; init; }
}

public record SkillRecord : SkillTemplateRecord { }
