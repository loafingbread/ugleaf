namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public static class EffectVariantFactory
{
    /// <summary>
    /// Creates the appropriate IEffectVariant from resolved EffectData.
    /// The variant stores the config (including any formula) and evaluates it at Compute() time.
    /// To add a new effect type: implement IEffectVariant, add a case here, and add JSON support.
    /// </summary>
    public static IEffectVariant CreateVariant(EffectData data)
    {
        return data.Type switch
        {
            "Attack" => new AttackEffectVariant(data.Config),
            "Heal" => new HealEffectVariant(data.Config),
            "Status" => CreateStatusVariant(data),
            _ => throw new NotSupportedException($"Unknown effect type: '{data.Type}'"),
        };
    }

    private static IEffectVariant CreateStatusVariant(EffectData data)
    {
        return data.Subtype switch
        {
            "Burn" => new BurnStatusVariant(data.Config),
            "Poison" => new PoisonStatusVariant(data.Config),
            _ => throw new NotSupportedException(
                $"Unknown status subtype: '{data.Subtype}'"
            ),
        };
    }

    /// <summary>
    /// Convenience method for resolving an inline EffectTemplateRef directly to an Effect,
    /// used when building UsableTemplates (effects embedded in usables are always inline).
    /// </summary>
    public static Effect CreateFromTemplateRef(
        EffectTemplateRef effectRef,
        Func<ReferenceId?, EffectData>? getEffectData = null
    )
    {
        EffectData data = EffectResolver.ToData(
            effectRef,
            getEffectData
                ?? (
                    _ =>
                        throw new NotSupportedException(
                            "Effect Ref/Override kinds require a registry lookup callback"
                        )
                )
        );
        IEffectVariant variant = CreateVariant(data);
        return new Effect(effectRef.ReferenceMetadata.ReferenceId, variant);
    }
}
