namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public static class EffectFactory
{
    public static Effect CreateEffect(EffectTemplate template)
    {
        return CreateEffectFromData(
            new EffectData(
                template.ReferenceId,
                template.Type.ToString(),
                template.Subtype,
                template.Variant,
                template.Name,
                template.Description,
                template.Tags,
                new EffectConfigData { Value = template.Value, Duration = template.Duration }
            )
        );
    }

    public static List<Effect> CreateEffectsFromData(List<EffectData> data)
    {
        return data.Select(CreateEffectFromData).ToList();
    }

    public static Effect CreateEffectFromData(EffectData data)
    {
        return Enum.Parse<EEffectType>(data.Type) switch
        {
            EEffectType.Status => CreateStatusEffectFromData(data),
            EEffectType.Attack => new AttackEffect(data),
            EEffectType.Heal => new HealEffect(data),
            _ => throw new NotSupportedException($"Effect type {data.Type} is not supported."),
        };
    }

    public static Effect CreateStatusEffectFromData(EffectData data)
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
