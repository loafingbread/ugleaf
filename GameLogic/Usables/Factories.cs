namespace GameLogic.Usables;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public static class UsableFactory
{
    public static Reference<UsableTemplate, Usable> CreateUsableReferenceFromRecord(
        ReferenceSpec record
    )
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateUsableTemplateFromReference(record);
            case EReferenceKind.Inline:
                return CreateUsableTemplateFromInline(record);
            case EReferenceKind.Override:
                return CreateUsableTemplateFromOverride(record);
            case EReferenceKind.Instance:
                return CreateInstanceFromReference(record);
            default:
                throw new NotImplementedException();
        }
    }

    public static Reference<UsableTemplate, Usable> CreateUsableTemplateFromReference(
        ReferenceSpec record
    )
    {
        var refSpec = record as RefSpec;
        if (refSpec is null)
        {
            throw new InvalidOperationException("Ref spec is not a usable template");
        }

        return new Reference<UsableTemplate, Usable>(
            refSpec.Metadata,
            new UsableTemplate(refSpec.Metadata, null, null),
            null
        );
    }

    public static Reference<UsableTemplate, Usable> CreateUsableTemplateFromInline(
        ReferenceSpec record
    )
    {
        var inlineSpec = record as InlineSpec<UsableTemplateSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a usable template");
        }

        return new Reference<UsableTemplate, Usable>(
            inlineSpec.Metadata,
            new UsableTemplate(inlineSpec.Metadata, inlineSpec.Template, null),
            null
        );
    }

    public static Reference<UsableTemplate, Usable> CreateUsableTemplateFromOverride(
        ReferenceSpec record
    )
    {
        var overrideSpec = record as OverrideSpec<UsableTemplateSpec, UsableOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a usable template");
        }

        return new Reference<UsableTemplate, Usable>(
            overrideSpec.Metadata,
            new UsableTemplate(overrideSpec.Metadata, null, overrideSpec.Override),
            null
        );
    }

    public static Reference<UsableTemplate, Usable> CreateInstanceFromReference(
        ReferenceSpec record
    )
    {
        var instanceSpec = record as InstanceSpec<UsableTemplateSpec, UsableInstanceSpec>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a usable template");
        }

        return new Reference<UsableTemplate, Usable>(
            instanceSpec.Metadata,
            null,
            new Usable(instanceSpec.Metadata, instanceSpec.InstanceId, instanceSpec.Instance)
        );
    }


    public static Usable CreateUsableFromTemplate(UsableTemplate template)
    {
        return template.Instantiate();
    }

    // public static Usable CreateUsableFromRecord(UsableInstanceSpec record)
    // {
    //     return new Usable(
    //         GameLogic.Registry.Ids.Instance(record.InstanceId),
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         TargetingFactory.CreateFromRecord(record.Targeter),
    //         record.Effects.Select(EffectFactory.CreateEffectFromRecord).ToList()
    //     );
    // }

    // public static UsableTemplate CreateUsableTemplateFromRecord(UsableTemplateSpec record)
    // {
    //     return new UsableTemplate(
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         TargetingFactory.CreateFromRecord(record.Targeter),
    //         record.Effects.Select(EffectFactory.CreateEffectFromRecord).ToList()
    //     );
    // }
}
