namespace GameLogic.Entities.Skills;

using GameLobic.Usables;
using GameLogic.Targeting;

public interface ISkillData
{
    public string Id { get; }
    public string Name { get; }
    // public string UsableTypeId { get; }
    public TargeterConfig? Targeter { get; }
    public UsableConfig? Usable { get; }
}

public record SkillConfig : ISkillData
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    // public required UsableConfig Usable { get; init; }
    public TargeterConfig? Targeter { get; init; }
    public UsableConfig? Usable { get; init; }
}
