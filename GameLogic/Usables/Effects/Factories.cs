namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public static class EffectFactory
{
    public static EffectTemplate CreateEffectTemplateFromRecord(EffectTemplateRecord record)
    {
        return new EffectTemplate(
            GameLogic.Registry.Ids.Template(record.TemplateId),
            Enum.Parse<EEffectType>(record.Type),
            record.Subtype,
            record.Variant,
            record.Name,
            record.Description,
            record.Tags,
            record.Config.Value,
            record.Config.Duration
        );
    }

    public static IEffect CreateEffectFromRecord(EffectRecord record)
    {
        return record.Type switch
        {
            "Status" => CreateStatusEffectFromRecord(record),
            "Attack" => new AttackEffect(
                GameLogic.Registry.Ids.Instance(record.InstanceId),
                GameLogic.Registry.Ids.Template(record.TemplateId),
                Enum.Parse<EEffectType>(record.Type),
                record.Subtype,
                record.Variant,
                record.Name,
                record.Description,
                record.Tags,
                record.Config.Value
            ),
            "Heal" => new HealEffect(
                GameLogic.Registry.Ids.Instance(record.InstanceId),
                GameLogic.Registry.Ids.Template(record.TemplateId),
                Enum.Parse<EEffectType>(record.Type),
                record.Subtype,
                record.Variant,
                record.Name,
                record.Description,
                record.Tags,
                record.Config.Value
            ),
            _ => throw new NotSupportedException($"Effect type {record.Type} is not supported."),
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

    private static IEffect CreateStatusEffectFromRecord(EffectRecord record)
    {
        return record.Subtype switch
        {
            "Burn" => new BurnStatusEffect(
                new InstanceId(record.InstanceId),
                new TemplateId(record.TemplateId),
                Enum.Parse<EEffectType>(record.Type),
                record.Subtype,
                record.Variant,
                record.Name,
                record.Description,
                record.Tags,
                record.Config.Value,
                record.Config.Duration
            ),
            "Poison" => new PoisonStatusEffect(
                new InstanceId(record.InstanceId),
                new TemplateId(record.TemplateId),
                Enum.Parse<EEffectType>(record.Type),
                record.Subtype,
                record.Variant,
                record.Name,
                record.Description,
                record.Tags,
                record.Config.Value,
                record.Config.Duration
            ),
            _ => throw new NotSupportedException(
                $"Effect subtype {record.Subtype} is not supported."
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
