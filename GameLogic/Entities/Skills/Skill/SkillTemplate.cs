namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Utils;

public record SkillTemplate : IDeepCopyable<SkillTemplate>
{
    public required ReferenceId ReferenceId { get; init; }
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();

    public TargeterData Targeter { get; private set; } = new();

    public List<UsableData> Usables { get; private set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public SkillTemplate(TemplateRefBase templateRef)
    {
        this.ReferenceId = templateRef.ReferenceMetadata.ReferenceId;
    }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public SkillTemplate(
        ReferenceId? referenceId,
        string name,
        string description,
        List<string> tags,
        TargeterData targeter,
        List<UsableData> usables
    )
    {
        this.ReferenceId = referenceId ?? Ids.NewReferenceId();
        this.Name = name;
        this.Description = description;
        this.Tags = tags;
        this.Targeter = targeter;
        this.Usables = usables;
    }

    public SkillTemplate DeepCopy()
    {
        return new SkillTemplate(
            null,
            this.Name,
            this.Description,
            this.Tags,
            this.Targeter.DeepCopy(),
            this.Usables.Select(usable => usable.DeepCopy()).ToList()
        );
    }
}
