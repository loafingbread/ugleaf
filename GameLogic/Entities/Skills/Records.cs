namespace GameLogic.Entities.Skills;

using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillTemplateRecord
{
    public required string TemplateId { get; init; }

    public required string Name { get; init; }
    public string Description { get; init; } = "";
    public List<string> Tags { get; init; } = new();

    public required TargeterRecord Targeter { get; init; }
    public List<UsableRecord> Usables { get; init; } = new();
}

public record SkillRecord : SkillTemplateRecord
{
    public string InstanceId { get; init; } = "";
}
