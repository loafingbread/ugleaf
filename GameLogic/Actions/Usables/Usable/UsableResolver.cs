namespace GameLogic.Actions.Usables.Usable;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Actions.Effects;

public static class UsableResolver
{
    public static List<UsableTemplate> ToTemplates(
        List<UsableTemplateRef> usableTemplateRefs,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate
    )
    {
        return usableTemplateRefs
            .Select(usableTemplateRef => ToTemplate(usableTemplateRef, getUsableTemplate))
            .ToList();
    }

    public static UsableTemplate ToTemplate(
        UsableTemplateRef usableTemplateRef,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate
    )
    {
        if (!TypedReferenceSpecValidator.IsTemplateRefValid(usableTemplateRef))
        {
            throw new InvalidOperationException("Usable reference spec is valid");
        }

        switch (usableTemplateRef.TemplateKind)
        {
            case ETemplateKind.Inline:
            {
                return new UsableTemplate(
                    usableTemplateRef.ReferenceMetadata.ReferenceId,
                    usableTemplateRef.ReferenceMetadata.DependencyId,
                    usableTemplateRef.TemplateValue!.DefaultName,
                    usableTemplateRef.TemplateValue!.DefaultDescription,
                    [.. usableTemplateRef.TemplateValue!.DefaultTags],
                    usableTemplateRef.TemplateValue!.DefaultTargeter,
                    [.. usableTemplateRef.TemplateValue!.DefaultEffects]
                );
            }
            case ETemplateKind.Ref:
            {
                UsableTemplate dependencyRef = getUsableTemplate(
                    usableTemplateRef.ReferenceMetadata.DependencyId
                );

                return dependencyRef;
            }
            case ETemplateKind.Override:
            {
                UsableTemplate dependencyRef = getUsableTemplate(
                    usableTemplateRef.ReferenceMetadata.DependencyId
                );

                return usableTemplateRef.TemplatePatch!.ApplyTo(dependencyRef);
            }
        }

        throw new InvalidOperationException("Invalid skill template spec kind");
    }

    public static List<Usable> ToUsables(
        List<UsableStateRef> usableStateRefs,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate,
        Func<ReferenceId?, UsableState> getUsableState
    )
    {
        return usableStateRefs
            .Select(usableStateRef => ToUsable(usableStateRef, getUsableTemplate, getUsableState))
            .ToList();
    }

    public static Usable ToUsable(
        UsableStateRef usableStateRef,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate,
        Func<ReferenceId?, UsableState> getUsableState
    )
    {
        UsableState usableState = getUsableState(usableStateRef.ReferenceMetadata.ReferenceId);
        UsableTemplate usableTemplate = getUsableTemplate(
            usableStateRef.ReferenceMetadata.DependencyId
        );

        return new Usable(usableTemplate, usableState);
    }

    public static List<UsableState> ToStates(
        List<UsableStateRef> usableStateRefs,
        Func<ReferenceId?, UsableState> getUsableState
    )
    {
        return usableStateRefs
            .Select(usableStateRef => ToState(usableStateRef, getUsableState))
            .ToList();
    }

    public static UsableState ToState(
        UsableStateRef usableStateRef,
        Func<ReferenceId?, UsableState> getUsableState
    )
    {
        if (!TypedReferenceSpecValidator.IsInstanceRefValid(usableStateRef))
        {
            throw new InvalidOperationException("Usable state reference spec is valid");
        }

        Targeter targeter = new Targeter(usableStateRef.InstanceState!.Targeter);
        List<IEffect> effects = usableStateRef
            .InstanceState!.Effects.Select(effectStateData => new Effect())
            .ToList();

        return new UsableState(
            usableStateRef.ReferenceMetadata.ReferenceId,
            usableStateRef.ReferenceMetadata.DependencyId,
            usableStateRef.InstanceState!.Name,
            usableStateRef.InstanceState!.Description,
            [.. usableStateRef.InstanceState!.Tags],
            targeter,
            effects
        );
    }
}
