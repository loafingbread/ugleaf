namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Usable;

public class SkillState
{
    public ReferenceId ReferenceId { get; set; } = Ids.NewReferenceId();
    public ReferenceId? DependencyId { get; set; } = null;

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public ITargeter Targeter { get; set; } = new NoTargeter(new Position(0, 0, 0));
    public List<IUsable> Usables { get; set; } = new();

    public SkillState(
        ReferenceId? referenceId,
        ReferenceId? dependencyId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<IUsable> usables
    )
    {
        this.ReferenceId = referenceId ?? Ids.NewReferenceId();
        this.DependencyId = dependencyId;
        this.Name = name;
        this.Description = description;
        this.Tags = tags;
        this.Targeter = targeter;
        this.Usables = usables;
    }

    public SkillState DeepCopy()
    {
        return new SkillState(
            null,
            this.DependencyId,
            this.Name,
            this.Description,
            [.. this.Tags],
            this.Targeter.DeepCopy(),
            this.Usables.Select((IUsable usable) => usable.DeepCopy()).ToList()
        );
    }
}
