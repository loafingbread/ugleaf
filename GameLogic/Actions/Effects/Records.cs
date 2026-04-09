namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public record EffectData : IDeepCopyable<EffectData>
{
    public required ReferenceId ReferenceId { get; init; }
    public string Type { get; set; } = "";
    public string Subtype { get; set; } = "";
    public string Variant { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public EffectConfigData Config { get; set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public EffectData(ReferenceSpec spec)
    {
        this.ReferenceId = spec.ReferenceMetadata.ReferenceId;
    }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public EffectData() { }

    public EffectData DeepCopy()
    {
        return new EffectData()
        {
            ReferenceId = Ids.NewReferenceId(),
            Type = this.Type,
            Subtype = this.Subtype,
            Variant = this.Variant,
            Name = this.Name,
            Description = this.Description,
            Tags = [.. this.Tags],
            Config = this.Config.DeepCopy(),
        };
    }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public EffectData(
        ReferenceId referenceId,
        string type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        EffectConfigData config
    )
    {
        this.ReferenceId = referenceId;
        this.Type = type;
        this.Subtype = subtype;
        this.Variant = variant;
        this.Name = name;
        this.Description = description;
        this.Tags = tags;
        this.Config = config;
    }

    public void Resolve(ReferenceSpec spec, Func<ReferenceId?, EffectData> getEffectData)
    {
        switch (spec.Metadata.Kind)
        {
            case ETemplateKind.Ref:
                this.ResolveReference(spec, getEffectData);
                break;
            case ETemplateKind.Inline:
                this.ResolveInline(spec);
                break;
            case ETemplateKind.Override:
                this.ResolveOverride(spec, getEffectData);
                break;
            case ETemplateKind.Instance:
                this.ResolveInstance(spec);
                break;
            default:
                throw new InvalidOperationException("Invalid effect spec kind");
        }
    }

    private void ResolveReference(ReferenceSpec spec, Func<ReferenceId?, EffectData> getEffectData)
    {
        EffectData data = getEffectData(spec.Metadata.ReferenceId);

        // TODO: Remove already set fields from base class
        this.Type = data.Type;
        this.Subtype = data.Subtype;
        this.Variant = data.Variant;
        this.Name = data.Name;
        this.Description = data.Description;
        this.Tags = [.. data.Tags];
        this.Config = data.Config.DeepCopy();
    }

    private void ResolveInline(ReferenceSpec spec)
    {
        var inlineSpec = spec as InlineSpec<EffectTemplateSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not an effect template");
        }

        this.Type = inlineSpec.Template.Type;
        this.Subtype = inlineSpec.Template.Subtype;
        this.Variant = inlineSpec.Template.Variant;
        this.Name = inlineSpec.Template.Name;
        this.Description = inlineSpec.Template.Description;
        this.Tags = [.. inlineSpec.Template.Tags];
        this.Config = inlineSpec.Template.Config.DeepCopy();
    }

    private void ResolveOverride(ReferenceSpec spec, Func<ReferenceId?, EffectData> getEffectData)
    {
        var overrideSpec = spec as OverrideSpec<EffectTemplateSpec, EffectOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not an effect template");
        }

        EffectData data = getEffectData(overrideSpec.Metadata.DependencyId);

        this.Type = overrideSpec.Override.Type ?? data.Type;
        this.Subtype = overrideSpec.Override.Subtype ?? data.Subtype;
        this.Variant = overrideSpec.Override.Variant ?? data.Variant;
        this.Name = overrideSpec.Override.Name ?? data.Name;
        this.Description = overrideSpec.Override.Description ?? data.Description;
        this.Tags = overrideSpec.Override.Tags ?? data.Tags;
        this.Config = overrideSpec.Override.Config ?? data.Config.DeepCopy();
    }

    private void ResolveInstance(ReferenceSpec spec)
    {
        var instanceSpec = spec as InstanceSpec<EffectTemplateSpec, EffectInstanceSpec>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not an effect instance");
        }

        this.Type = instanceSpec.Instance.Type;
        this.Subtype = instanceSpec.Instance.Subtype;
        this.Variant = instanceSpec.Instance.Variant;
        this.Name = instanceSpec.Instance.Name;
        this.Description = instanceSpec.Instance.Description;
        this.Tags = [.. instanceSpec.Instance.Tags];
        this.Config = instanceSpec.Instance.Config.DeepCopy();
    }
}

public record EffectConfigData
{
    public int Value { get; init; } = 0;
    public int Duration { get; init; } = 0;

    public EffectConfigData DeepCopy()
    {
        return new EffectConfigData { Value = this.Value, Duration = this.Duration };
    }
}

public enum EEffectType
{
    None,
    Attack,
    Heal,
    Buff,
    Debuff,
    Status,
}
