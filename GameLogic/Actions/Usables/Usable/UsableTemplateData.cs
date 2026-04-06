namespace GameLogic.Actions.Usables.Usable;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Actions.Effects;

public record UsableTemplateRef : TemplateRef<UsableTemplateSpec, UsableTemplatePatch> { }

public record UsableTemplateSpec
{
    public required string DefaultName { get; init; } = "";
    public required string DefaultDescription { get; init; } = "";
    public required List<string> DefaultTags { get; init; } = new();
    public required List<string> Tags { get; init; } = new();
    public required TargeterData DefaultTargeter { get; init; }
    public required List<EffectTemplateData> DefaultEffects { get; init; } = new();
}

public record UsableTemplatePatch
{
    public string? DefaultName { get; init; }
    public string? DefaultDescription { get; init; }
    public List<string>? DefaultTags { get; init; }
    public TargeterData? DefaultTargeter { get; init; }
    public List<EffectTemplateData>? DefaultEffects { get; init; }

    public UsableTemplate ApplyTo(UsableTemplate baseTemplate)
    {
        return new UsableTemplate(
            baseTemplate.ReferenceId,
            baseTemplate.DependencyId,
            this.DefaultName ?? baseTemplate.DefaultName,
            this.DefaultDescription ?? baseTemplate.DefaultDescription,
            this.DefaultTags ?? baseTemplate.DefaultTags,
            this.DefaultTargeter ?? baseTemplate.DefaultTargeter,
            this.DefaultEffects ?? baseTemplate.DefaultEffects
        );
    }
}
