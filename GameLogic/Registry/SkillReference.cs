namespace GameLogic.Registry;

using GameLogic.Entities.Skills;
using GameLogic.Actions.Usables;

public class SkillReference : ReferenceBase<SkillTemplate, SkillData>
{
    public SkillReference(IRegistry registry, ReferenceSpec spec, SkillTemplate? value)
        : base(registry, spec, value) { }

    protected override SkillData InitData()
    {
        return new SkillData(this.Spec);
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

        if (this.Spec.ReferenceMetadata.Kind == ETemplateKind.Instance)
        {
            Skill skill = new Skill(this.Data);
            this.Registry.TryAdd(
                (IReference<object, ReferenceSpec>)this,
                this.Spec.ReferenceMetadata.ReferenceId
            );
            return;
        }

        SkillTemplate skillTemplate = new SkillTemplate(this.Data);
        this.Registry.TryAdd(
            (IReference<object, ReferenceSpec>)this,
            this.Spec.ReferenceMetadata.ReferenceId
        );
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
