namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;

public record CharacterRecord : IStatBlockRecord
{
    public required string InstanceId { get; init; }
    public required string TemplateId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
    public List<StatRecord> Stats { get; init; } = new();
    public List<SkillRecord> Skills { get; init; } = new();
}

public record CharacterTemplateRecord
{
    public required string Id { get; init; }
    public required CharacterMetadataRecord Metadata { get; init; }
    public required CharacterConfigRecord Config { get; init; }
}

public record CharacterMetadataRecord
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
}

public record CharacterConfigRecord : IStatBlockRecord
{
    public required List<StatRecord> Stats { get; init; }
    public required List<SkillRecord> Skills { get; init; }
}
