using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public record UsableTemplateSpec
{
    public required string Name { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();
    public required TargeterData Targeter { get; init; }
    public required List<ReferenceSpec> Effects { get; init; } = new();
}

public record UsableOverrideSpec
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public TargeterData? Targeter { get; init; }
    public List<ReferenceSpec>? Effects { get; init; }
}

public record UsableInstanceSpec : UsableTemplateSpec { }

public record UsableData : IDeepCopyable<UsableData>
{
    public required ReferenceId ReferenceId { get; init; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();

    public TargeterData Targeter { get; set; } = new();
    public List<EffectData> Effects { get; set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public UsableData(ReferenceSpec spec)
    {
        this.ReferenceId = spec.ReferenceMetadata.ReferenceId;
    }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public UsableData() { }

    public UsableData DeepCopy()
    {
        return new UsableData()
        {
            ReferenceId = Ids.NewReferenceId(),
            Name = this.Name,
            Description = this.Description,
            Tags = [.. this.Tags],
            Targeter = this.Targeter.DeepCopy(),
            Effects = [.. this.Effects.Select(effect => effect.DeepCopy())],
        };
    }

    public void Resolve(
        ReferenceSpec spec,
        Func<ReferenceId?, UsableData> getUsableData,
        Func<ReferenceId?, EffectData> getEffectData
    )
    {
        switch (spec.ReferenceMetadata.Kind)
        {
            case ETemplateKind.Ref:
            {
                this.ResolveReference(spec, getUsableData);
                return;
            }
            case ETemplateKind.Inline:
            {
                this.ResolveInline(spec, getEffectData);
                return;
            }
            case ETemplateKind.Override:
            {
                this.ResolveOverride(spec, getEffectData, getUsableData);
                return;
            }
            case ETemplateKind.Instance:
            {
                this.ResolveInstance(spec, getEffectData);
                return;
            }
            default:
            {
                throw new InvalidOperationException("Invalid reference kind");
            }
        }
    }

    private void ResolveReference(ReferenceSpec spec, Func<ReferenceId?, UsableData> getUsableData)
    {
        UsableData usableData = getUsableData(spec.ReferenceMetadata.ReferenceId);

        this.Name = usableData.Name;
        this.Description = usableData.Description;
        this.Tags = [.. usableData.Tags];
        this.Targeter = usableData.Targeter;
        this.Effects = [.. usableData.Effects];
    }

    private void ResolveInline(ReferenceSpec spec, Func<ReferenceId?, EffectData> getEffectData)
    {
        var inlineSpec = spec as TypedReferenceSpec<UsableTemplateSpec, UsableOverrideSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a usable template");
        }

        this.Name = inlineSpec.Template.Name;
        this.Description = inlineSpec.Template.Description;
        this.Tags = [.. inlineSpec.Template.Tags];
        this.Targeter = inlineSpec.Template.Targeter;
        this.Effects = this.ResolveEffects(inlineSpec.Template.Effects, getEffectData);
    }

    private void ResolveOverride(
        ReferenceSpec spec,
        Func<ReferenceId?, EffectData> getEffectData,
        Func<ReferenceId?, UsableData> getUsableData
    )
    {
        var overrideSpec = spec as OverrideSpec<UsableTemplateSpec, UsableOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a usable template");
        }

        UsableData usableData = getUsableData(overrideSpec.Metadata.DependencyId);

        this.Name = overrideSpec.Override.Name ?? usableData.Name;
        this.Description = overrideSpec.Override.Description ?? usableData.Description;
        this.Tags = overrideSpec.Override.Tags ?? usableData.Tags;
        this.Targeter = overrideSpec.Override.Targeter ?? usableData.Targeter;
        this.Effects = overrideSpec.Override.Effects is not null
            ? this.ResolveEffects(overrideSpec.Override.Effects, getEffectData)
            : [.. usableData.Effects];
    }

    private void ResolveInstance(ReferenceSpec spec, Func<ReferenceId?, EffectData> getEffectData)
    {
        var instanceSpec = spec as InstanceSpec<UsableTemplateSpec, UsableInstanceSpec>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a usable template");
        }

        this.Name = instanceSpec.Instance.Name;
        this.Description = instanceSpec.Instance.Description;
        this.Tags = [.. instanceSpec.Instance.Tags];
        this.Targeter = instanceSpec.Instance.Targeter;
        this.Effects = this.ResolveEffects(instanceSpec.Instance.Effects, getEffectData);
    }

    private List<EffectData> ResolveEffects(
        List<ReferenceSpec> effectSpecs,
        Func<ReferenceId?, EffectData> getEffectData
    )
    {
        List<EffectData> effectDatas = new();
        foreach (var effectSpec in effectSpecs)
        {
            EffectData effectData = getEffectData(effectSpec.Metadata.ReferenceId);
            effectDatas.Add(effectData);
        }

        return effectDatas;
    }
}
