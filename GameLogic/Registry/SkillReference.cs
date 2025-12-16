namespace GameLogic.Registry;

using GameLogic.Entities.Skills;
using GameLogic.Usables;

public class SkillReference : ReferenceBase<SkillTemplate, SkillData>
{
    public SkillReference(
        EntityRegistry<SkillTemplate> entityRegistry,
        ReferenceSpec spec,
        SkillTemplate? value
    )
        : base(entityRegistry, spec, value)
    {
        this.Data = new SkillData(spec);
    }

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data.Resolve(this.Spec, this.GetSkillData(registry), this.GetUsableData(registry));
    }

    public override void Initialize()
    {
        if (this.Data is null)
        {
            throw new InvalidOperationException(
                "Skill data should be resolved before initializing the reference"
            );
        }

        if (this.Spec.Metadata.Kind == EReferenceKind.Instance)
        {
            Skill skill = new Skill(this.Data);
            this.entityRegistry.TryAdd(skill, this.Spec.Metadata.ReferenceId);
            return;
        }

        SkillTemplate skillTemplate = new SkillTemplate(this.Data);
        this.entityRegistry.TryAdd(skillTemplate, this.Spec.Metadata.ReferenceId);
    }

    protected Func<ReferenceId?, SkillData> GetSkillData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var skillRef =
                registry.GetReference(referenceId) as IReference<SkillTemplate, SkillData>
                ?? throw new InvalidOperationException("Skill reference not found");

            skillRef.Resolve(registry);
            return skillRef.GetData();
        };
    }

    protected Func<ReferenceId?, UsableData> GetUsableData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var usableRef =
                registry.GetReference(referenceId) as IReference<UsableTemplate, UsableData>
                ?? throw new InvalidOperationException("Usable reference not found");

            usableRef.Resolve(registry);
            return usableRef.GetData();
        };
    }
}

public static class ReferenceFactory
{
    public static IReference CreateReferenceFromRecord(
        ReferenceSpec record,
        EntitiesRegistry entitiesRegistry
    )
    {
        switch (record.Metadata.TemplateType)
        {
            case ETemplateType.Skill:
                return new SkillReference(entitiesRegistry.Skills, record, null);
            default:
                throw new InvalidOperationException("Invalid template type");
        }
    }
}
