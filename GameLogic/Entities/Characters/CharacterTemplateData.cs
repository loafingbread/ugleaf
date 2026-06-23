namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;

public record CharacterTemplateData : IStatBlockRecord
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public List<StatRecord> Stats { get; init; } = new();
    public List<SkillTemplateData> Skills { get; init; } = new();
}
