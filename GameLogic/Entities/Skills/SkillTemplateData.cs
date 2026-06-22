namespace GameLogic.Entities.Skills;

using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillTemplateData
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public TargeterRecord? Targeter { get; init; }
    public List<UsableTemplateData> Usables { get; init; } = new();
}
