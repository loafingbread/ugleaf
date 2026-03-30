namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Usable;

public record SkillTemplateRef : TemplateRef<SkillTemplateSpec, SkillTemplatePatch> { }

public record SkillTemplateSpec
{
    public string DefaultName { get; private set; } = "";
    public string DefaultDescription { get; private set; } = "";
    public List<string> DefaultTags { get; private set; } = new();

    public TargeterData DefaultTargeter { get; private set; } = new();

    public List<UsableTemplateRef> DefaultUsables { get; private set; } = new();
}

public record SkillTemplatePatch
{
    public string? DefaultName { get; init; }
    public string? DefaultDescription { get; init; }
    public List<string>? DefaultTags { get; init; } = new();
    public TargeterData? DefaultTargeter { get; init; }
    public List<UsableTemplateRef>? DefaultUsables { get; init; } = new();

    public SkillTemplate ApplyTo(
        SkillTemplate baseTemplate,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate
    )
    {
        return new SkillTemplate(
            baseTemplate.ReferenceId,
            baseTemplate.DependencyId,
            this.DefaultName ?? baseTemplate.DefaultName,
            this.DefaultDescription ?? baseTemplate.DefaultDescription,
            this.DefaultTags ?? baseTemplate.DefaultTags,
            this.DefaultTargeter ?? baseTemplate.DefaultTargeter,
            this.DefaultUsables is not null
                ? this
                    .DefaultUsables.Select(usableTemplateRef =>
                        getUsableTemplate(usableTemplateRef.ReferenceMetadata.ReferenceId)
                    )
                    .ToList()
                : baseTemplate.DefaultUsables
        );
    }
}
