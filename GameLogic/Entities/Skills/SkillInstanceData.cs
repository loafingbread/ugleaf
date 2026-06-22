namespace GameLogic.Entities.Skills;

public record SkillInstanceData
{
    public required string TemplateId { get; init; }
    public int Level { get; init; } = 1;
    public int CurrentCooldown { get; init; } = 0;
}
