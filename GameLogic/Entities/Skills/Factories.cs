using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public static class SkillFactory
{
    public static IRegistryReference CreateSkillReferenceFromRecord(ReferenceUnionSpec record)
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateSkillTemplateFromReference(record);
            case EReferenceKind.Inline:
                return CreateSkillTemplateFromInline(record);
            case EReferenceKind.Override:
                return CreateSkillTemplateFromOverride(record);
            case EReferenceKind.Instance:
                return CreateInstanceFromReference(record);
            default:
                throw new NotImplementedException();
        }
    }

    public static Reference<SkillTemplate, Skill> CreateSkillTemplateFromReference(
        ReferenceUnionSpec record
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
        ReferenceUnionSpec record
    )
    {
        var inlineSpec = record as InlineSpec<SkillTemplateRecord>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a skill template");
        }

        return new SkillTemplateReference(
            inlineSpec.Metadata,
            new SkillTemplate(inlineSpec.Metadata, inlineSpec.Template, null),
            null
        );
    }

    public static Reference<SkillTemplate, Skill> CreateSkillTemplateFromOverride(
        ReferenceUnionSpec record
    )
    {
        var overrideSpec = record as OverrideSpec<SkillTemplateRecord, SkillOverrideRecord>;
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

    public static Reference<SkillTemplate, Skill> CreateInstanceFromReference(
        ReferenceUnionSpec record
    )
    {
        var instanceSpec = record as InstanceSpec<SkillTemplateRecord, SkillRecord>;
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
    // public static Skill CreateSkillFromRecord(ReferenceUnionSpec record)
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

    // public static SkillTemplate CreateSkillTemplateFromRecord(SkillTemplateRecord record)
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
