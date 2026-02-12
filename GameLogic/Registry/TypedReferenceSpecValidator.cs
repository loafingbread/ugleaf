namespace GameLogic.Registry;

public static class TypedReferenceSpecValidator
{
    public static bool IsValid<TValue, TPatch>(TypedReferenceSpec<TValue, TPatch>? referenceSpec)
    {
        if (referenceSpec is null)
        {
            return false;
        }

        bool isInline = referenceSpec.ReferenceMetadata.Kind == ETemplateKind.Inline;
        bool isOverride = referenceSpec.ReferenceMetadata.Kind == ETemplateKind.Override;
        bool isInstance = referenceSpec.ReferenceMetadata.Kind == ETemplateKind.Instance;
        bool isRef = referenceSpec.ReferenceMetadata.Kind == ETemplateKind.Ref;
        bool hasDependency = referenceSpec.ReferenceMetadata.DependencyId is not null;

        if (isInline)
        {
            return referenceSpec.Value is not null;
        }
        else if (isOverride)
        {
            return referenceSpec.Patch is not null;
        }
        else if (isInstance && !hasDependency)
        {
            // Instance without a dependency must have a value.
            // It should be resolved from the value of the instance
            return referenceSpec.Value is not null;
        }
        else if (isInstance && hasDependency)
        {
            // Instance with a dependency must not have a value.
            // It should be resolved from the reference of the dependency.
            return referenceSpec.Value is null;
        }
        else if (isRef)
        {
            return true;
        }

        throw new InvalidOperationException("Invalid reference kind");
    }
}
