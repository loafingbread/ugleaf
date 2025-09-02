namespace GameLogic.Entities.Skills;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillRecord
{
    public required string InstanceId { get; init; }
    public required string TemplateId { get; init; }

    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterConfig Targeter { get; init; }
    public required List<UsableConfig> Usables { get; init; }
}

public record SkillTemplateRecord
{
    public required string Id { get; init; }
    public required SkillMetadataRecord Metadata { get; init; }
    public required SkillConfigRecord Config { get; init; }
}

public record SkillMetadataRecord
{
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
}

public record SkillConfigRecord
{
    public required TargeterConfig? Targeter { get; init; }
    public required List<UsableConfig> Usables { get; init; }
}
