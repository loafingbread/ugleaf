namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Utils;

public class SkillTemplate
    : IReferenceUnion,
        ITemplate<SkillOverrideRecord>,
        IDeepCopyable<SkillTemplate>
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
    public SkillOverrideRecord? TemplateOverride { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();

    public ITargeter Targeter { get; set; } = new NoTargeter(new Position(0, 0, 0));
    public List<Usable> Usables { get; set; } = new();

    public SkillTemplate(
        ReferenceUnionMetadata referenceMetadata,
        SkillTemplateRecord? templateRecord,
        SkillOverrideRecord? templateOverride
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

    private void ApplyTemplateRecord(SkillTemplateRecord? templateRecord)
    {
        if (templateRecord is null)
        {
            return;
        }

        this.Name = templateRecord.Name;
        this.Description = templateRecord.Description;
        this.Tags = [.. templateRecord.Tags];
        this.Targeter = TargetingFactory.CreateFromRecord(templateRecord.Targeter);
        this.Usables = this.CreateUsablesFromReferences(templateRecord.Usables);
    }

    // Copy constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SkillTemplate(SkillTemplate template)
    {
        this.ApplyTemplate(template);
    }
#pragma warning restore CS8618

    public void LoadReferences(IRegistry registry)
    {
        if (this.ReferenceMetadata.Kind == EReferenceKind.Inline)
        {
            // Inline templates do not need to be loaded
            return;
        }

        registry.TryGetValue<Reference<SkillTemplate, Skill>>(
            this.ReferenceMetadata,
            out Reference<SkillTemplate, Skill> referenceValue
        );

        SkillTemplate? template = referenceValue.Template;
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

    protected void ApplyTemplate(SkillTemplate template)
    {
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Targeter = template.Targeter.DeepCopy();
        this.Usables = template.Usables.DeepCopyList();
    }

    private void ApplyOverrides(SkillOverrideRecord? templateOverride)
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
        this.Usables = templateOverride.Usables is null
            ? this.Usables
            : this.CreateUsablesFromReferences(templateOverride.Usables);
    }

    protected List<Usable> CreateUsablesFromReferences(List<ReferenceUnionSpec> usables)
    {
        return usables
            .Select(
                (ReferenceUnionSpec record) =>
                {
                    Reference<UsableTemplate, Usable> usableReference =
                        UsableFactory.CreateUsableReferenceFromRecord(record);
                    return usableReference.Template?.Instantiate();
                }
            )
            .Select(
                (Usable? usable) =>
                    usable is not null
                        ? usable
                        : throw new InvalidOperationException(
                            "Usable template should not be null while loading usable files"
                        )
            )
            .ToList();
    }

    public Skill Instantiate()
    {
        return SkillFactory.CreateSkillFromTemplate(this);
    }

    public SkillTemplate DeepCopy()
    {
        return new SkillTemplate(this);
    }
}

public class Skill : SkillTemplate, IInstance<SkillRecord>, IDeepCopyable<Skill>
{
    public InstanceId InstanceId { get; set; }
    public SkillRecord? InstanceState { get; set; }

    public Skill(
        ReferenceUnionMetadata referenceMetadata,
        InstanceId instanceId,
        SkillRecord? instanceState
    )
        : base(referenceMetadata, null, null)
    {
        if (referenceMetadata.Kind != EReferenceKind.Instance)
        {
            throw new InvalidOperationException("Skill reference is not an instance");
        }

        this.InstanceId = instanceId;
        this.InstanceState = instanceState;
    }

    public Skill(Skill skill)
        : base((skill as SkillTemplate).DeepCopy())
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

        registry.TryGetValue<Reference<SkillTemplate, Skill>>(
            this.ReferenceMetadata,
            out Reference<SkillTemplate, Skill> referenceValue
        );

        SkillTemplate? skillTemplate = referenceValue.Template;
        if (skillTemplate is null)
        {
            throw new InvalidOperationException(
                "Skill template should not be null if instance state is null"
            );
        }

        this.ApplyTemplate(skillTemplate);
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
        this.Targeter = TargetingFactory.CreateFromRecord(this.InstanceState.Targeter);
        this.Usables = this.CreateUsablesFromReferences(this.InstanceState.Usables);
    }

    public new Skill DeepCopy()
    {
        return new Skill(this);
    }

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usables.Count > 0;
}
