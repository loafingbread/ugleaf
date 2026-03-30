namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Usable;

public static class SkillResolver
{
    public static List<SkillTemplate> ToTemplates(
        List<SkillTemplateRef> skillTemplateRefs,
        Func<ReferenceId?, SkillTemplate> getSkillTemplate,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate
    )
    {
        return skillTemplateRefs
            .Select(skillTemplateRef =>
                ToTemplate(skillTemplateRef, getSkillTemplate, getUsableTemplate)
            )
            .ToList();
    }

    public static SkillTemplate ToTemplate(
        SkillTemplateRef skillTemplateRef,
        Func<ReferenceId?, SkillTemplate> getSkillTemplate,
        Func<ReferenceId?, UsableTemplate> getUsableTemplate
    )
    {
        if (!TypedReferenceSpecValidator.IsTemplateRefValid(skillTemplateRef))
        {
            throw new InvalidOperationException("Skill template reference spec is valid");
        }

        switch (skillTemplateRef.TemplateKind)
        {
            case ETemplateKind.Inline:
            {
                return new SkillTemplate(
                    skillTemplateRef.ReferenceMetadata.ReferenceId,
                    skillTemplateRef.ReferenceMetadata.DependencyId,
                    skillTemplateRef.TemplateValue!.DefaultName,
                    skillTemplateRef.TemplateValue!.DefaultDescription,
                    [.. skillTemplateRef.TemplateValue!.DefaultTags],
                    skillTemplateRef.TemplateValue!.DefaultTargeter,
                    UsableResolver.ToTemplates(
                        skillTemplateRef.TemplateValue!.DefaultUsables,
                        getUsableTemplate
                    )
                );
            }
            case ETemplateKind.Ref:
            {
                SkillTemplate dependencyRef = getSkillTemplate(
                    skillTemplateRef.ReferenceMetadata.DependencyId
                );

                return dependencyRef;
            }
            case ETemplateKind.Override:
            {
                SkillTemplate dependencyRef = getSkillTemplate(
                    skillTemplateRef.ReferenceMetadata.DependencyId
                );

                return skillTemplateRef.TemplatePatch!.ApplyTo(dependencyRef, getUsableTemplate);
            }
        }

        throw new InvalidOperationException("Invalid skill template spec kind");
    }

    public static List<Skill> ToSkills(
        List<SkillStateRef> skillStateRefs,
        Func<ReferenceId?, SkillTemplate> getSkillTemplate,
        Func<ReferenceId?, SkillState> getSkillState,
        Func<ReferenceId?, IUsable> getUsable
    )
    {
        return skillStateRefs
            .Select(skillStateRef =>
                ToSkill(skillStateRef, getSkillTemplate, getSkillState, getUsable)
            )
            .ToList();
    }

    public static Skill ToSkill(
        SkillStateRef skillStateRef,
        Func<ReferenceId?, SkillTemplate> getSkillTemplate,
        Func<ReferenceId?, SkillState> getSkillState,
        Func<ReferenceId?, IUsable> getUsable
    )
    {
        SkillState skillState = ToState(skillStateRef, getSkillState, getUsable);
        SkillTemplate skillTemplate = getSkillTemplate(
            skillStateRef.ReferenceMetadata.DependencyId
        );

        return new Skill(skillTemplate, skillState);
    }

    public static List<SkillState> ToStates(
        List<SkillStateRef> skillStateRefs,
        Func<ReferenceId?, SkillState> getSkillState,
        Func<ReferenceId?, IUsable> getUsable
    )
    {
        return skillStateRefs
            .Select(skillStateRef => ToState(skillStateRef, getSkillState, getUsable))
            .ToList();
    }

    public static SkillState ToState(
        SkillStateRef skillStateRef,
        Func<ReferenceId?, SkillState> getSkillState,
        Func<ReferenceId?, IUsable> getUsable
    )
    {
        if (!TypedReferenceSpecValidator.IsInstanceRefValid(skillStateRef))
        {
            throw new InvalidOperationException("Skill state reference spec is valid");
        }

        Targeter targeter = new Targeter(skillStateRef.InstanceState!.Targeter);
        List<IUsable> usables = skillStateRef
            .InstanceState.Usables.Select(usableStateData =>
                getUsable(usableStateData.ReferenceMetadata.ReferenceId)
            )
            .ToList();

        return new SkillState(
            skillStateRef.ReferenceMetadata.ReferenceId,
            skillStateRef.ReferenceMetadata.DependencyId,
            skillStateRef.InstanceState!.Name,
            skillStateRef.InstanceState!.Description,
            [.. skillStateRef.InstanceState!.Tags],
            targeter,
            usables
        );
    }
}
