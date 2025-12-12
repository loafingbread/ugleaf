namespace GameLogic.Usables;

using GameLogic.Entities;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;
using GameLogic.Utils;

public class UsableTemplate : ITemplate<UsableOverrideSpec>
{
    public ReferenceMetadata ReferenceMetadata { get; set; }
    public UsableOverrideSpec? TemplateOverride { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public ITargeter Targeter { get; set; }
    public List<IEffect> Effects { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UsableTemplate(
        ReferenceMetadata referenceMetadata,
        UsableTemplateSpec? templateRecord,
        UsableOverrideSpec? templateOverride
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

        this.ReferenceMetadata = referenceMetadata;
        this.TemplateOverride = templateOverride;
        this.ApplyTemplateRecord(templateRecord);
    }
#pragma warning restore CS8618

    private void ApplyTemplateRecord(UsableTemplateSpec? templateRecord)
    {
        if (templateRecord is null)
        {
            return;
        }

        this.Name = templateRecord.Name;
        this.Description = templateRecord.Description;
        this.Tags = [.. templateRecord.Tags];
        this.Targeter = TargetingFactory.CreateFromRecord(templateRecord.Targeter);
        this.Effects = this.CreateEffectsFromReferences(templateRecord.Effects);
    }

    protected List<IEffect> CreateEffectsFromReferences(List<ReferenceSpec> effects)
    {
        return effects
            .Select(
                (ReferenceSpec record) =>
                {
                    Reference<EffectTemplate, IEffect> effectReference =
                        EffectFactory.CreateEffectReferenceFromRecord(record);
                    return effectReference.Template?.Instantiate();
                }
            )
            .Select(
                (IEffect? effect) =>
                {
                    return effect is not null
                        ? effect
                        : throw new InvalidOperationException(
                            "Effect template should not be null while loading effect files"
                        );
                }
            )
            .ToList();
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UsableTemplate(UsableTemplate template)
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

        registry.TryGetValue<Reference<UsableTemplate, Usable>>(
            this.ReferenceMetadata,
            out Reference<UsableTemplate, Usable> referenceValue
        );

        UsableTemplate? template = referenceValue.Template;
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

    protected void ApplyTemplate(UsableTemplate template)
    {
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Targeter = template.Targeter.DeepCopy();
        this.Effects = template.Effects.DeepCopyList();
    }

    public void ApplyOverrides(UsableOverrideSpec? templateOverride)
    {
        if (templateOverride is null)
        {
            return;
        }

        this.TemplateOverride = templateOverride;
        this.Name = templateOverride.Name ?? this.Name;
        this.Description = templateOverride.Description ?? this.Description;
        this.Tags = templateOverride.Tags ?? this.Tags;
        this.Targeter = templateOverride.Targeter is null
            ? this.Targeter
            : TargetingFactory.CreateFromRecord(templateOverride.Targeter);
        this.Effects = templateOverride.Effects is null
            ? this.Effects
            : this.CreateEffectsFromReferences(templateOverride.Effects);
    }

    public Usable Instantiate()
    {
        return new Usable(this);
    }

    public UsableTemplate DeepCopy()
    {
        return new UsableTemplate(this);
    }
}

public class Usable : UsableTemplate, IUsable, IInstance<UsableInstanceSpec>, IDeepCopyable<Usable>
{
    public InstanceId InstanceId { get; set; }
    public UsableInstanceSpec? InstanceState { get; set; }

    public Usable(
        ReferenceMetadata referenceMetadata,
        InstanceId? instanceId,
        UsableInstanceSpec? instanceState
    )
        : base(referenceMetadata, null, null)
    {
        if (referenceMetadata.Kind != EReferenceKind.Instance)
        {
            throw new InvalidOperationException("Usable reference is not an instance");
        }

        this.InstanceId = Ids.Instance();
        this.InstanceState = instanceState;
    }

    public Usable(Usable usable)
        : base((usable as UsableTemplate).DeepCopy())
    {
        this.InstanceState = usable.InstanceState;
        this.InstanceId = Ids.Instance();
    }

    // Create a new usable from a template
    public Usable(UsableTemplate template)
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

        registry.TryGetValue<Reference<UsableTemplate, Usable>>(
            this.ReferenceMetadata,
            out Reference<UsableTemplate, Usable> referenceValue
        );

        UsableTemplate? template = referenceValue.Template;
        if (template is null)
        {
            throw new InvalidOperationException(
                "Template reference should not be null if instance state is null"
            );
        }

        this.ApplyTemplate(template);
    }

    public void ApplyInstanceState()
    {
        if (this.InstanceState is null)
        {
            return;
        }

        this.Name = this.InstanceState.Name;
        this.Description = this.InstanceState.Description;
        this.Tags = [.. this.InstanceState.Tags];
        this.Targeter = TargetingFactory.CreateFromRecord(this.InstanceState.Targeter);
        this.Effects = this.CreateEffectsFromReferences(this.InstanceState.Effects);
    }

    public new Usable DeepCopy()
    {
        return new Usable(this);
    }

    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets)
    {
        // TODO: Implement usable result calculation
        List<UsableResult> results = new();
        foreach (Entity target in targets)
        {
            results.Add(new UsableResult(this, user, target));
        }

        return results;
    }
}

public sealed class UsableTemplateReference : Reference<UsableTemplate>
{
    public UsableTemplateReference(ReferenceMetadata referenceMetadata, UsableTemplate? value)
        : base(referenceMetadata, value) { }
}

public sealed class UsableInstanceReference : Reference<Usable>
{
    public UsableInstanceReference(ReferenceMetadata metadata, Usable? value)
        : base(metadata, value) { }
}
