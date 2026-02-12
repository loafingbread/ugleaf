namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Usables;

public static class SkillResolver
{
    public static SkillData ToData(
        ReferenceSpec untypedRef,
        Func<ReferenceId?, SkillData> getSkillData,
        Func<ReferenceId?, UsableData> getUsableData
    )
    {
        TypedReferenceSpec<SkillSpec, SkillPatch>? typedRef =
            untypedRef as TypedReferenceSpec<SkillSpec, SkillPatch>;
        if (!TypedReferenceSpecValidator.IsValid(typedRef))
        {
            throw new InvalidOperationException("Reference spec is valid");
        }

        switch (typedRef!.ReferenceMetadata.Kind)
        {
            case ETemplateKind.Inline:
            {
                return new SkillData(
                    typedRef.ReferenceMetadata.ReferenceId,
                    typedRef.Value!.Name,
                    typedRef.Value!.Description,
                    [.. typedRef.Value!.Tags],
                    typedRef.Value!.Targeter,
                    [
                        .. typedRef
                            .Value!.Usables.Select(usable =>
                                getUsableData(usable.ReferenceMetadata.ReferenceId)
                            )
                            .ToList(),
                    ]
                );
            }
            case ETemplateKind.Ref:
            {
                SkillData refData = getSkillData(typedRef.ReferenceMetadata.ReferenceId);
                return refData;
            }
            case ETemplateKind.Override:
            {
                SkillData refData = getSkillData(typedRef.ReferenceMetadata.DependencyId);
                SkillData overridedData = typedRef.Patch!.ApplyTo(refData, getUsableData);
                return overridedData;
            }
            case ETemplateKind.Instance:
            {
                if (typedRef.ReferenceMetadata.DependencyId is null)
                {
                    return new SkillData(
                        typedRef.ReferenceMetadata.ReferenceId,
                        typedRef.Value!.Name,
                        typedRef.Value!.Description,
                        [.. typedRef.Value!.Tags],
                        typedRef.Value!.Targeter,
                        [
                            .. typedRef.Value!.Usables.Select(usable =>
                                getUsableData(usable.ReferenceMetadata.ReferenceId)
                            ),
                        ]
                    );
                }

                SkillData refData = getSkillData(typedRef.ReferenceMetadata.DependencyId);
                return new SkillData(
                    typedRef.ReferenceMetadata.ReferenceId,
                    refData.Name,
                    refData.Description,
                    [.. refData.Tags],
                    refData.Targeter,
                    [.. refData.Usables]
                );
            }
            default:
            {
                throw new InvalidOperationException("Invalid skill spec kind");
            }
        }
    }
}
