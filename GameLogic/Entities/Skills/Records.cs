namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillSpecBase { }

public record SkillTemplateSpec : SkillSpecBase
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterData Targeter { get; init; }

    // TODO: Do I need to include typing enforcement for usable type here?

    public required List<ReferenceSpec> Usables { get; init; } = new();
}

public record SkillOverrideSpec : SkillSpecBase
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public TargeterData? Targeter { get; init; }
    public List<ReferenceSpec>? Usables { get; init; }
}

public record SkillInstanceSpec : SkillTemplateSpec { }

public record SkillSpec : SkillTemplateSpec { }

public record SkillData
{
    public ReferenceId ReferenceId { get; init; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();

    public TargeterData Targeter { get; set; } = new();

    public List<UsableData> Usables { get; set; } = new();

    public SkillData(ReferenceSpec spec)
    {
        this.ReferenceId = spec.Metadata.ReferenceId;
    }

    public void Resolve(
        ReferenceSpec spec,
        Func<ReferenceId?, SkillData> getSkillData,
        Func<ReferenceId?, UsableData> getUsableData
    )
    {
        switch (spec.Metadata.Kind)
        {
            case EReferenceKind.Ref:
            {
                SkillData skillRef = getSkillData(spec.Metadata.ReferenceId);

                this.Name = skillRef.Name;
                this.Description = skillRef.Description;
                this.Tags = skillRef.Tags;
                this.Targeter = skillRef.Targeter;
                this.Usables = skillRef.Usables;
                return;
            }
            case EReferenceKind.Inline:
            {
                var inlineSpec = spec as InlineSpec<SkillTemplateSpec>;
                if (inlineSpec is null)
                {
                    throw new InvalidOperationException("Inline spec is not a skill template");
                }

                this.Name = inlineSpec.Template.Name;
                this.Description = inlineSpec.Template.Description;
                this.Tags = [.. inlineSpec.Template.Tags];
                this.Targeter = inlineSpec.Template.Targeter;
                this.Usables = this.ResolveUsables(inlineSpec.Template.Usables, getUsableData);
                return;
            }
            case EReferenceKind.Override:
            {
                var overrideSpec = spec as OverrideSpec<SkillTemplateSpec, SkillOverrideSpec>;
                if (overrideSpec is null)
                {
                    throw new InvalidOperationException("Override spec is not a skill template");
                }

                SkillData skillData = getSkillData(spec.Metadata.ReferenceId);

                this.Name = overrideSpec.Override.Name ?? skillData.Name;
                this.Description = overrideSpec.Override.Description ?? skillData.Description;
                this.Tags = overrideSpec.Override.Tags ?? skillData.Tags;
                this.Targeter = overrideSpec.Override.Targeter ?? skillData.Targeter;
                this.Usables = overrideSpec.Override.Usables is not null
                    ? this.ResolveUsables(overrideSpec.Override.Usables, getUsableData)
                    : [.. skillData.Usables];
                return;
            }
            case EReferenceKind.Instance:
            {
                var instanceSpec = spec as InstanceSpec<SkillTemplateSpec, SkillInstanceSpec>;
                if (instanceSpec is null)
                {
                    throw new InvalidOperationException("Instance spec is not a skill instance");
                }

                this.Name = instanceSpec.Instance.Name;
                this.Description = instanceSpec.Instance.Description;
                this.Tags = [.. instanceSpec.Instance.Tags];
                this.Targeter = instanceSpec.Instance.Targeter;
                this.Usables = this.ResolveUsables(instanceSpec.Instance.Usables, getUsableData);
                return;
            }
            default:
            {
                throw new InvalidOperationException("Invalid skill spec kind");
            }
        }
    }

    public List<UsableData> ResolveUsables(
        List<ReferenceSpec> usableSpecs,
        Func<ReferenceId, UsableData> getUsableData
    )
    {
        List<UsableData> usableDatas = new();
        foreach (var usableSpec in usableSpecs)
        {
            UsableData usableData = getUsableData(usableSpec.Metadata.ReferenceId);
            usableDatas.Add(usableData);
        }

        return usableDatas;
    }
}
