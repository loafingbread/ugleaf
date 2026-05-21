namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public static class EffectResolver
{
    public static EffectTemplate ToTemplate(
        EffectTemplateRef effectTemplateRef,
        Func<ReferenceId?, EffectTemplate> getEffectTemplate
    )
    {
        if (!TypedReferenceSpecValidator.IsTemplateRefValid(effectTemplateRef))
        {
            throw new InvalidOperationException("Effect template reference spec is not valid");
        }

        switch (effectTemplateRef.TemplateKind)
        {
            case ETemplateKind.Inline:
            {
                return new EffectTemplate(
                    effectTemplateRef.ReferenceMetadata.ReferenceId,
                    effectTemplateRef.TemplateValue!.DefaultName,
                    effectTemplateRef.TemplateValue!.DefaultDescription,
                    effectTemplateRef.TemplateValue!.DefaultType,
                    effectTemplateRef.TemplateValue!.DefaultSubtype,
                    effectTemplateRef.TemplateValue!.DefaultConfig
                );
            }
            case ETemplateKind.Ref:
            {
                // TODO: Consider replacing all instances of this with generic,
                // since this seems repeated in all other types
                EffectTemplate dependencyRef = getEffectTemplate(
                    effectTemplateRef.ReferenceMetadata.DependencyId
                );

                return dependencyRef;
            }
            case ETemplateKind.Override:
            {
                // TODO: Consider replacing all instances of this with generic,
                // since this seems repeated in all other types
                EffectTemplate dependencyRef = getEffectTemplate(
                    effectTemplateRef.ReferenceMetadata.DependencyId
                );

                return effectTemplateRef.TemplatePatch!.ApplyTo(dependencyRef);
            }
            default:
            {
                throw new InvalidOperationException("Invalid effect template spec kind");
            }
        }

        throw new InvalidOperationException("Invalid effect template spec kind");
    }

    /// <summary>
    /// Resolves a reference spec to flat EffectData, handling Inline / Ref / Override kinds.
    /// Follows the same pattern as StatResolver.ToData().
    /// </summary>
    public static EffectData ToData(Ref untypedRef, Func<ReferenceId?, EffectData> getEffectData)
    {
        var typedRef =
            untypedRef as TemplateRef<EffectData, EffectPatch>
            ?? throw new InvalidOperationException(
                $"Expected TemplateRef<EffectData, EffectPatch>, got {untypedRef.GetType().Name}"
            );

        return typedRef.TemplateKind switch
        {
            ETemplateKind.Inline => typedRef.TemplateValue!,
            ETemplateKind.Ref => getEffectData(typedRef.ReferenceMetadata.ReferenceId),
            ETemplateKind.Override => typedRef.TemplatePatch!.ApplyTo(
                getEffectData(typedRef.ReferenceMetadata.DependencyId)
            ),
            _ => throw new InvalidOperationException(
                $"Invalid effect template kind: {typedRef.TemplateKind}"
            ),
        };
    }
}
