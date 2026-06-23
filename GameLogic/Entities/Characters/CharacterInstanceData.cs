namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;

public record CharacterInstanceData
{
    public required string TemplateId { get; init; }
    public List<SkillInstanceData> Skills { get; init; } = new();
}
