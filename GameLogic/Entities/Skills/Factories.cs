using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public class SkillArtifact 
{
    public SkillTemplate? Template { get; set; }
    public Skill? Instance { get; set; }

    public SkillArtifact(SkillTemplate? template, Skill? instance)
    {
        this.Template = template;
        this.Instance = instance;
    }

    public static SkillArtifact FromTemplate(SkillTemplate template)
    {
        return new SkillArtifact(template, null);
    }

    public static SkillArtifact FromInstance(Skill instance)
    {
        return new SkillArtifact(null, instance);
    }
}

public static class SkillFactory
{
    public static SkillTemplateSpec MergeTemplateAndOverride(
        SkillTemplateSpec templateSpec,
        SkillOverrideSpec overrideSpec
    )
    {
        return new SkillTemplateSpec
        {
            Name = overrideSpec.Name ?? templateSpec.Name,
            Description = overrideSpec.Description ?? templateSpec.Description,
            Tags = overrideSpec.Tags ?? templateSpec.Tags,
            Targeter = overrideSpec.Targeter ?? templateSpec.Targeter,
            Usables = overrideSpec.Usables ?? templateSpec.Usables,
        };
    }

    public static SkillArtifact CreateSkillArtifactFromData(SkillData data, EReferenceKind kind)
    {
        switch (kind)
        {
            case EReferenceKind.Ref:
            case EReferenceKind.Inline:
            case EReferenceKind.Override:
                return SkillArtifact.FromTemplate(new SkillTemplate(data));
            case EReferenceKind.Instance:
                return SkillArtifact.FromInstance(new Skill(data));
            default: 
                throw new NotImplementedException("Invalid skill artifact kind");
        }
    }

    public static IReference<SkillTemplate, Skill> CreateSkillInstanceSpecFromRecord(
        ReferenceSpec spec
    )
    {
        switch (spec.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateSkillTemplateFromReference(spec);
            case EReferenceKind.Inline:
                return CreateSkillTemplateFromInline(spec);
            case EReferenceKind.Override:
                return CreateSkillTemplateFromOverride(spec);
            case EReferenceKind.Instance:
                return CreateInstanceFromReference(spec);
            default:
                throw new NotImplementedException();
        }
    }

    public static Reference<SkillTemplate, Skill> CreateSkillTemplateFromReference(
        ReferenceSpec record
    )
    {
        var refSpec = record as RefSpec;
        if (refSpec is null)
        {
            throw new InvalidOperationException("Ref spec is not a skill template");
        }

        return new Reference<SkillTemplate, Skill>(
            refSpec.Metadata,
            new SkillTemplate(refSpec.Metadata, null, null),
            null
        );
    }

    public static Reference<SkillTemplate, Skill> CreateSkillTemplateFromInline(
        ReferenceSpec record
    )
    {
        var inlineSpec = record as InlineSpec<SkillTemplateSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a skill template");
        }

        return new SkillTemplateSpec(
            inlineSpec.Metadata,
            new SkillTemplate(inlineSpec.Metadata, inlineSpec.Template, null),
            null
        );
    }

    public static Reference<SkillTemplate, Skill> CreateSkillTemplateFromOverride(
        ReferenceSpec record
    )
    {
        var overrideSpec = record as OverrideSpec<SkillTemplateSpec, SkillOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a skill template");
        }

        return new Reference<SkillTemplate, Skill>(
            overrideSpec.Metadata,
            new SkillTemplate(overrideSpec.Metadata, null, overrideSpec.Override),
            null
        );
    }

    public static Reference<SkillTemplate, Skill> CreateInstanceFromReference(ReferenceSpec record)
    {
        var instanceSpec = record as InstanceSpec<SkillTemplateSpec, SkillInstanceSpec>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a skill template");
        }

        return new Reference<SkillTemplate, Skill>(
            instanceSpec.Metadata,
            null,
            new Skill(instanceSpec.Metadata, instanceSpec.InstanceId, instanceSpec.Instance)
        );
    }

    public static Skill CreateSkillFromTemplate(SkillTemplate template)
    {
        return template.Instantiate();
    }
    // public static Skill CreateSkillFromRecord(ReferenceSpec record)
    // {
    //     return new Skill(
    //         record.Metadata.InstanceId,
    //         record.Metadata,
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         TargetingFactory.CreateFromRecord(record.Targeter),
    //         record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
    //     );
    // }

    // public static SkillTemplate CreateSkillTemplateFromRecord(SkillTemplateSpec record)
    // {
    //     return new SkillTemplate(
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         TargetingFactory.CreateFromRecord(record.Targeter),
    //         record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
    //     );
    // }
}
