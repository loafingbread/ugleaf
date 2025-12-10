namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public static class EffectFactory
{
    public static Reference<EffectTemplate, IEffect> CreateEffectReferenceFromRecord(
        ReferenceSpec record
    )
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateEffectTemplateFromReference(record);
            case EReferenceKind.Inline:
                return CreateEffectTemplateFromInline(record);
            case EReferenceKind.Override:
                return CreateEffectTemplateFromOverride(record);
            case EReferenceKind.Instance:
                return CreateInstanceFromReference(record);
            default:
                throw new NotImplementedException();
        }
    }

    public static Reference<EffectTemplate, IEffect> CreateEffectTemplateFromReference(
        ReferenceSpec record
    )
    {
        var refSpec = record as RefSpec;
        if (refSpec is null)
        {
            throw new InvalidOperationException("Ref spec is not a effect template");
        }

        return new Reference<EffectTemplate, IEffect>(refSpec.Metadata, null, null);
    }

    public static Reference<EffectTemplate, IEffect> CreateEffectTemplateFromInline(
        ReferenceSpec record
    )
    {
        var inlineSpec = record as InlineSpec<EffectTemplateSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a effect template");
        }

        return new Reference<EffectTemplate, IEffect>(
            inlineSpec.Metadata,
            new EffectTemplate(inlineSpec.Metadata, inlineSpec.Template, null),
            null
        );
    }

    public static Reference<EffectTemplate, IEffect> CreateEffectTemplateFromOverride(
        ReferenceSpec record
    )
    {
        var overrideSpec = record as OverrideSpec<EffectTemplateSpec, EffectOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a effect template");
        }

        return new Reference<EffectTemplate, IEffect>(
            overrideSpec.Metadata,
            new EffectTemplate(overrideSpec.Metadata, null, overrideSpec.Override),
            null
        );
    }

    public static Reference<EffectTemplate, IEffect> CreateInstanceFromReference(
        ReferenceSpec record
    )
    {
        var instanceSpec = record as InstanceSpec<EffectTemplateSpec, EffectInstanceSpec>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a effect template");
        }

        return new Reference<EffectTemplate, IEffect>(
            instanceSpec.Metadata,
            null,
            CreateEffectFromInstanceSpec(instanceSpec)
        );
    }

    public static IEffect CreateEffectFromInstanceSpec(
        InstanceSpec<EffectTemplateSpec, EffectInstanceSpec> instanceSpec
    )
    {
        if (instanceSpec.Instance is null)
        {
            throw new InvalidOperationException("Instance is null");
        }

        return instanceSpec.Instance.Type switch
        {
            "Status" => CreateStatusEffectFromRecord(instanceSpec),
            "Attack" => new AttackEffect(
                instanceSpec.Metadata,
                instanceSpec.InstanceId,
                instanceSpec.Instance
            ),
            "Heal" => new HealEffect(
                instanceSpec.Metadata,
                instanceSpec.InstanceId,
                instanceSpec.Instance
            ),
            _ => throw new NotSupportedException(
                $"Effect type {instanceSpec.Instance.Type} is not supported."
            ),
        };
    }

    public static IEffect CreateEffectFromTemplate(EffectTemplate template)
    {
        return template.Type switch
        {
            EEffectType.Status => CreateStatusEffectFromTemplate(template),
            EEffectType.Attack => new AttackEffect(template),
            EEffectType.Heal => new HealEffect(template),
            _ => throw new NotSupportedException($"Effect type {template.Type} is not supported."),
        };
    }

    private static IEffect CreateStatusEffectFromRecord(
        InstanceSpec<EffectTemplateSpec, EffectInstanceSpec> instanceSpec
    )
    {
        if (instanceSpec.Instance is null)
        {
            throw new InvalidOperationException("Instance is null");
        }

        return instanceSpec.Instance.Subtype switch
        {
            "Burn" => new BurnStatusEffect(
                instanceSpec.Metadata,
                instanceSpec.InstanceId,
                instanceSpec.Instance
            ),
            "Poison" => new PoisonStatusEffect(
                instanceSpec.Metadata,
                instanceSpec.InstanceId,
                instanceSpec.Instance
            ),
            _ => throw new NotSupportedException(
                $"Effect subtype {instanceSpec.Instance.Subtype} is not supported."
            ),
        };
    }

    private static IEffect CreateStatusEffectFromTemplate(EffectTemplate template)
    {
        return template.Subtype switch
        {
            "Burn" => new BurnStatusEffect(template),
            "Poison" => new PoisonStatusEffect(template),
            _ => throw new NotSupportedException(
                $"Effect subtype {template.Subtype} is not supported."
            ),
        };
    }
}
