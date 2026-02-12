namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public class SkillState
{
    public required ReferenceId ReferenceId { get; init; } = Ids.NewReferenceId();
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();
    public ITargeter Targeter { get; private set; } = new NoTargeter(new Position(0, 0, 0));
    public List<IUsable> Usables { get; private set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public SkillState(
        ReferenceId? referenceId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<IUsable> usables
    )
    {
        this.ReferenceId = referenceId ?? Ids.NewReferenceId();
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
            this.Name,
            this.Description,
            this.Tags,
            this.Targeter.DeepCopy(),
            this.Usables.Select(usable => usable.DeepCopy()).ToList()
        );
    }
}
