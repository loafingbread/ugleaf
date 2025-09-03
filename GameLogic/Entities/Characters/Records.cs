namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;

public record CharacterTemplateRecord : IStatBlockRecord
{
    public required string TemplateId { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public List<string> Tags { get; init; } = new();
    public List<StatRecord> Stats { get; init; } = new();
    public List<SkillRecord> Skills { get; init; } = new();
}

public record CharacterRecord : CharacterTemplateRecord
{
    public string InstanceId { get; init; } = "";
}
