namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

public static class StatResolver
{
    public static StatData ToData(
        ReferenceSpec untypedRef,
        Func<ReferenceId?, StatData> getStatData
    )
    {
        TypedReferenceSpec<StatSpec, StatPatch>? typedRef =
            untypedRef as TypedReferenceSpec<StatSpec, StatPatch>;
        if (!TypedReferenceSpecValidator.IsValid(typedRef))
        {
            throw new InvalidOperationException("Reference spec is valid");
        }

        // Ignoring nullability warnings because we know the reference is valid
        // due to the validation above.
        switch (typedRef!.ReferenceMetadata.Kind)
        {
            case ETemplateKind.Inline:
            {
                return new StatData(
                    typedRef.ReferenceMetadata.ReferenceId,
                    typedRef.Value!.ValueModel,
                    typedRef.Value!.Metadata,
                    typedRef.Value!.Capabilities
                );
            }
            case ETemplateKind.Ref:
            {
                StatData refData = getStatData(typedRef.ReferenceMetadata.ReferenceId);
                return refData;
            }
            case ETemplateKind.Override:
            {
                StatData refData = getStatData(typedRef.ReferenceMetadata.DependencyId);
                StatData overridedData = typedRef.Patch!.ApplyTo(refData);
                return overridedData;
            }
            case ETemplateKind.Instance:
            {
                if (typedRef.ReferenceMetadata.DependencyId is null)
                {
                    // Create instance from inlined value. No dependency.
                    return new StatData(
                        typedRef.ReferenceMetadata.ReferenceId,
                        typedRef.Value!.ValueModel,
                        typedRef.Value!.Metadata,
                        typedRef.Value.Capabilities
                    );
                }

                // Create instance from dependency.
                StatData refData = getStatData(typedRef.ReferenceMetadata.DependencyId);
                return new StatData(
                    typedRef.ReferenceMetadata.ReferenceId,
                    refData.ValueModel,
                    refData.Metadata,
                    refData.Capabilities
                );
            }
            default:
            {
                throw new InvalidOperationException("Invalid reference kind");
            }
        }
    }
}
