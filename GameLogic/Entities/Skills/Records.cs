namespace GameLogic.Entities.Skills;

using GameLogic.Entities.Skills.Skill;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public record SkillTemplateSpec
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterData Targeter { get; init; }

    // TODO: Do I need to include typing enforcement for usable type here?

    public required List<ReferenceSpec> Usables { get; init; } = new();
}

public record SkillPatch
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public TargeterData? Targeter { get; init; }
    public List<ReferenceSpec>? Usables { get; init; }

    public SkillData ApplyTo(SkillData baseData, Func<ReferenceId?, UsableData> getUsableData)
    {
        return new SkillData()
        {
            ReferenceId = baseData.ReferenceId,
            Name = this.Name ?? baseData.Name,
            Description = this.Description ?? baseData.Description,
            Tags = this.Tags ?? baseData.Tags,
            Targeter = this.Targeter ?? baseData.Targeter,
            Usables = this.Usables is not null
                ? this
                    .Usables.Select(usable => getUsableData(usable.ReferenceMetadata.ReferenceId))
                    .ToList()
                : baseData.Usables,
        };
    }
}
