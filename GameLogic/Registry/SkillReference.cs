namespace GameLogic.Registry;

using GameLogic.Entities.Skills;

public class SkillTemplateReference : ReferenceBase<SkillTemplate, SkillTemplateSpec>
{
    private SkillTemplateSpec? templateRecord { get; set; } = null;
    private SkillOverrideSpec? overrideRecord { get; set; } = null;
    private SkillTemplate? value { get; set; } = null;

    public SkillTemplateReference(EntityRegistry<SkillTemplate> entityRegistry, SkillTemplateSpec spec)
        : base(entityRegistry, spec) { }

    public override void ResolveDependencies(IRegistry registry)
    {
        // 1. Go through all nested objects and resolve them (create references for them e.g. usables). This will
        // allow us to get them by reference id later. Need to store any newly created ids to be able to resolve them later.
        // 2. Resolve all deps to template and override records
        // 3. Merge template and override records into a single template record
        if (this.Spec.Metadata.Kind == EReferenceKind.Inline)
        {
            var inlineSpec = this.Spec as InlineSpec<SkillTemplateSpec>;
            if (inlineSpec is null)
            {
                throw new InvalidOperationException("Inline spec is not a skill template");
            }

            this.templateRecord = new SkillTemplateSpec{
                Name = inlineSpec.Template.Name,
                Description = inlineSpec.Template.Description,
                Tags = [.. inlineSpec.Template.Tags],
                Targeter = inlineSpec.Template.Targeter,
                Usables = [.. inlineSpec.Template.Usables.Select(dep => dep.Resolve(registry))],
            };
        } else if (this.Spec.Metadata.Kind == EReferenceKind.Ref)
        {
            var refSpec = this.Spec as RefSpec;
            if (refSpec is null)
            {
                throw new InvalidOperationException("Ref spec is not a skill template");
            }
            
            this.templateRecord = 
        }
    }

    public override void Initialize()
    {
        if (this.templateRecord is null && this.overrideRecord is null)
        {
            throw new InvalidOperationException("Template or override record is required");
        }

        throw new NotImplementedException();
    }

}

public class SkillInstanceReference : ReferenceBase<Skill, SkillInstanceSpec>
{
    private SkillInstanceSpec? instanceRecord { get; set; } = null;
    private Skill? value { get; set; } = null;

    public SkillInstanceReference(SkillInstanceSpec spec)
        : base(spec) { }

    public override void ResolveDependencies(IRegistry registry)
    {
        throw new NotImplementedException();
    }

    public override void Initialize()
    {
        if (this.instanceRecord is null)
        {
            throw new InvalidOperationException("Instance record is required");
        }

        throw new NotImplementedException();
    }
}

public static class ReferenceFactory
{
    public static IReference CreateReferenceFromRecord(ReferenceSpec record, EntityRegistry<object> entityRegistry)
    {
        switch (record.Metadata.TemplateType)
        {
            case ETemplateType.Skill:
                return CreateSkillInstanceSpec(record);
            default:
                throw new InvalidOperationException("Invalid template type");
        }
    }

    private static IReference CreateSkillInstanceSpec(ReferenceSpec record)
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
            case EReferenceKind.Inline:
            case EReferenceKind.Override:
                return new SkillTemplateReference(record);
            case EReferenceKind.Instance:
                return new SkillInstanceReference(record);
            default:
                throw new InvalidOperationException("Invalid skill reference kind");
        }
    }
}