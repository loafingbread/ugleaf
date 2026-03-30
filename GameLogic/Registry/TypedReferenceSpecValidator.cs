namespace GameLogic.Registry;

public static class TypedReferenceSpecValidator
{
    public static bool IsTemplateRefValid<TValue, TPatch>(TemplateRef<TValue, TPatch>? templateRef)
    {
        if (templateRef is null)
        {
            return false;
        }

        bool isInline = templateRef.TemplateKind == ETemplateKind.Inline;
        bool isOverride = templateRef.TemplateKind == ETemplateKind.Override;
        bool isRef = templateRef.TemplateKind == ETemplateKind.Ref;

        if (isInline)
        {
            return templateRef.TemplateValue is not null;
        }
        else if (isOverride)
        {
            bool hasOverrideValue = templateRef.TemplateValue is not null;
            bool hasDependency = templateRef.ReferenceMetadata.DependencyId is not null;
            return hasOverrideValue && hasDependency;
        }
        else if (isRef)
        {
            bool hasDependency = templateRef.ReferenceMetadata.DependencyId is not null;
            return hasDependency;
        }

        throw new InvalidOperationException("Invalid reference kind");
    }

    public static bool IsInstanceRefValid<TState>(InstanceRef<TState>? instanceRef)
    {
        if (instanceRef is null)
        {
            return false;
        }

        return instanceRef.InstanceState is not null;
    }
}
