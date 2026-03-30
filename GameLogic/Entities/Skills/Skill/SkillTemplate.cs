namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Usable;
using GameLogic.Utils;

public class SkillTemplate
{
    public ReferenceId ReferenceId { get; set; } = Ids.NewReferenceId();
    public ReferenceId? DependencyId { get; set; }

    public string DefaultName { get; set; } = "";
    public string DefaultDescription { get; set; } = "";
    public List<string> DefaultTags { get; set; } = new();
    public TargeterData DefaultTargeter { get; set; } = new();
    public List<UsableTemplate> DefaultUsables { get; set; } = new();

    public SkillTemplate(
        ReferenceId referenceId,
        ReferenceId? dependencyId,
        string defaultName,
        string defaultDescription,
        List<string> defaultTags,
        TargeterData defaultTargeter,
        List<UsableTemplate> defaultUsables
    )
    {
        this.ReferenceId = referenceId;
        this.DependencyId = dependencyId;
        this.DefaultName = defaultName;
        this.DefaultDescription = defaultDescription;
        this.DefaultTags = defaultTags;
        this.DefaultTargeter = defaultTargeter;
        this.DefaultUsables = defaultUsables;
    }

    public SkillTemplate DeepCopy()
    {
        return new SkillTemplate(
            Ids.NewReferenceId(),
            this.DependencyId,
            this.DefaultName,
            this.DefaultDescription,
            this.DefaultTags,
            this.DefaultTargeter.DeepCopy(),
            this.DefaultUsables.Select(usable => usable.DeepCopy()).ToList()
        );
    }
}
