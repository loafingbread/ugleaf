namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Actions.Usables.Usable;
using GameLogic.Registry;
using GameLogic.Targeting;

public record SkillStateRef : InstanceRef<SkillStateSpec> { }

public record SkillStateSpec
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();
    public TargeterData Targeter { get; private set; } = new();
    public List<UsableStateRef> Usables { get; private set; } = new();
}
