namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public static class EffectResolver
{
    /// <summary>
    /// Resolves a reference spec to flat EffectData, handling Inline / Ref / Override kinds.
    /// Follows the same pattern as StatResolver.ToData().
    /// </summary>
    public static EffectData ToData(
        Ref untypedRef,
        Func<ReferenceId?, EffectData> getEffectData
    )
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
