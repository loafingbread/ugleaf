using GameLogic.Targeting;

namespace GameLogic.Usables.Effects;

using GameLogic.Registry;
using GameLogic.Utils;

public class EffectTemplate
    : IReferenceUnion,
        ITemplate<EffectOverrideRecord>,
        IDeepCopyable<EffectTemplate>
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
    public EffectOverrideRecord? TemplateOverride { get; set; }
    public EEffectType Type { get; set; }
    public string Subtype { get; set; }
    public string Variant { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }

    public int Value { get; set; }
    public int Duration { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EffectTemplate(
        ReferenceUnionMetadata referenceMetadata,
        EffectTemplateRecord? templateRecord,
        EffectOverrideRecord? templateOverride
    )
    {
        if (referenceMetadata.Kind == EReferenceKind.Instance)
        {
            throw new InvalidOperationException("Templates cannot be loaded from instances");
        }
        else if (referenceMetadata.Kind == EReferenceKind.Inline && templateRecord is null)
        {
            throw new InvalidOperationException("Inline templates must have a template record");
        }
        else if (referenceMetadata.Kind == EReferenceKind.Override && templateOverride is null)
        {
            throw new InvalidOperationException("Override templates must have a template override");
        }
        else if (referenceMetadata.Kind != EReferenceKind.Ref)
        {
            throw new InvalidOperationException("Invalid reference kind");
        }

        this.ReferenceMetadata = referenceMetadata;
        this.TemplateOverride = templateOverride;
        this.ApplyTemplateRecord(templateRecord);
    }
#pragma warning restore CS8618

    private void ApplyTemplateRecord(EffectTemplateRecord? templateRecord)
    {
        if (templateRecord is null)
        {
            return;
        }

        this.Type = Enum.Parse<EEffectType>(templateRecord.Type);
        this.Subtype = templateRecord.Subtype;
        this.Variant = templateRecord.Variant;
        this.Name = templateRecord.Name;
        this.Description = templateRecord.Description;
        this.Tags = [.. templateRecord.Tags];
        this.Value = templateRecord.Config.Value;
        this.Duration = templateRecord.Config.Duration;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EffectTemplate(EffectTemplate template)
    {
        this.ApplyTemplate(template);
    }
#pragma warning restore CS8618

    public void LoadReferences(IRegistry registry)
    {
        if (this.ReferenceMetadata.Kind == EReferenceKind.Inline)
        {
            return;
        }

        registry.TryGetValue<Reference<EffectTemplate, IEffect>>(
            this.ReferenceMetadata,
            out Reference<EffectTemplate, IEffect> referenceValue
        );

        EffectTemplate? template = referenceValue.Template;
        if (template is null)
        {
            throw new InvalidOperationException("Template reference should not be null");
        }

        if (this.ReferenceMetadata.Kind == EReferenceKind.Ref)
        {
            this.ApplyTemplate(template);
        }
        else if (this.ReferenceMetadata.Kind == EReferenceKind.Override)
        {
            this.ApplyTemplate(template);
            this.ApplyOverrides(this.TemplateOverride);
        }
    }

    protected void ApplyTemplate(EffectTemplate template)
    {
        this.Type = template.Type;
        this.Subtype = template.Subtype;
        this.Variant = template.Variant;
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Value = template.Value;
        this.Duration = template.Duration;
    }

    private void ApplyOverrides(EffectOverrideRecord? templateOverride)
    {
        if (templateOverride is null)
        {
            return;
        }

        this.TemplateOverride = templateOverride;
        this.Name = templateOverride.Name ?? this.Name;
        this.Description = templateOverride.Description ?? this.Description;
        this.Tags = templateOverride.Tags ?? this.Tags;

        if (templateOverride.Config is not null)
        {
            this.Value = templateOverride.Config.Value;
            this.Duration = templateOverride.Config.Duration;
        }
    }

    public EffectTemplate DeepCopy()
    {
        return new EffectTemplate(this);
    }

    public IEffect Instantiate()
    {
        return EffectFactory.CreateEffectFromTemplate(this);
    }
}

public abstract class Effect : EffectTemplate, IEffect, IInstance<EffectRecord>
{
    public InstanceId InstanceId { get; set; }
    public EffectRecord? InstanceState { get; set; }

    public Effect(
        ReferenceUnionMetadata referenceMetadata,
        InstanceId instanceId,
        EffectRecord? instanceState
    )
        : base(referenceMetadata, null, null)
    {
        if (referenceMetadata.Kind != EReferenceKind.Instance)
        {
            throw new InvalidOperationException("Effect reference is not an instance");
        }

        this.InstanceId = instanceId;
        this.InstanceState = instanceState;
    }

    public Effect(Effect effect)
        : base((effect as EffectTemplate).DeepCopy())
    {
        this.InstanceId = Ids.Instance();
    }

    public Effect(EffectTemplate template)
        : base(template)
    {
        this.InstanceId = Ids.Instance();
    }

    public new void LoadReferences(IRegistry registry)
    {
        if (this.InstanceState is not null)
        {
            this.ApplyInstanceState();
            return;
        }

        registry.TryGetValue<Reference<EffectTemplate, IEffect>>(
            this.ReferenceMetadata,
            out Reference<EffectTemplate, IEffect> referenceValue
        );

        EffectTemplate? template = referenceValue.Template;
        if (template is null)
        {
            throw new InvalidOperationException("Template reference should not be null");
        }

        this.ApplyTemplate(template);
    }

    private void ApplyInstanceState()
    {
        if (this.InstanceState is null)
        {
            return;
        }

        this.Name = this.InstanceState.Name;
        this.Description = this.InstanceState.Description;
        this.Tags = [.. this.InstanceState.Tags];
        this.Value = this.InstanceState.Config.Value;
        this.Duration = this.InstanceState.Config.Duration;
    }

    public new abstract IEffect DeepCopy();

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
    }
}
