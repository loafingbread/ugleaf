namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillTemplateSpec
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterData Targeter { get; init; }

    // TODO: Do I need to include typing enforcement for usable type here?

    public required List<ReferenceSpec> Usables { get; init; } = new();
}

public record SkillOverrideSpec : ITemplateOverride<SkillTemplateSpec>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public TargeterData? Targeter { get; init; }
    public List<ReferenceSpec>? Usables { get; init; }
}

public record SkillInstanceSpec : SkillTemplateSpec { }

public record SkillData
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterData Targeter { get; init; }

    public required List<UsableData> Usables { get; init; } = new();
}
