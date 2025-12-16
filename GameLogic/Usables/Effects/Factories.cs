namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public static class EffectFactory
{
    public static IEffect CreateEffect(EffectTemplate template)
    {
        return CreateEffectFromData(
            new EffectData
            {
                ReferenceId = template.ReferenceId,
                Type = template.Type.ToString(),
                Subtype = template.Subtype,
                Variant = template.Variant,
                Name = template.Name,
                Description = template.Description,
                Tags = template.Tags,
                Config = new EffectConfigData
                {
                    Value = template.Value,
                    Duration = template.Duration,
                },
            }
        );
    }

    public static List<IEffect> CreateEffectsFromData(List<EffectData> data)
    {
        return data.Select(CreateEffectFromData).ToList();
    }

    public static IEffect CreateEffectFromData(EffectData data)
    {
        return Enum.Parse<EEffectType>(data.Type) switch
        {
            EEffectType.Status => CreateStatusEffectFromData(data),
            EEffectType.Attack => new AttackEffect(data),
            EEffectType.Heal => new HealEffect(data),
            _ => throw new NotSupportedException($"Effect type {data.Type} is not supported."),
        };
    }

    public static IEffect CreateStatusEffectFromData(EffectData data)
    {
        return data.Subtype switch
        {
            "Burn" => new BurnStatusEffect(data),
            "Poison" => new PoisonStatusEffect(data),
            _ => throw new NotSupportedException(
                $"Effect subtype {data.Subtype} is not supported."
            ),
        };
    }
}
