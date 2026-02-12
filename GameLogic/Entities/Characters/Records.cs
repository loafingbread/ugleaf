namespace GameLogic.Entities.Characters;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public record CharacterTemplateSpec : IStatBlockSpec
{
    public required string Name { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();
    public required List<ReferenceSpec> Stats { get; init; } = new();
    public required List<ReferenceSpec> Skills { get; init; } = new();
}

public record CharacterOverrideSpec
{
    public string? Name { get; init; } = "";
    public string? Description { get; init; } = "";
    public List<string>? Tags { get; init; } = new();
    public List<ReferenceSpec>? Stats { get; init; } = new();
    public List<ReferenceSpec>? Skills { get; init; } = new();
}

public record CharacterData : IStatBlockData
{
    public required ReferenceId ReferenceId { get; init; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public List<StatData> Stats { get; init; } = new();
    public List<SkillData> Skills { get; set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public CharacterData(ReferenceSpec spec)
    {
        this.ReferenceId = spec.Metadata.ReferenceId;
    }

    public void Resolve(
        ReferenceSpec spec,
        Func<ReferenceId?, CharacterData> getCharacterData,
        Func<ReferenceId?, StatData> getStatData,
        Func<ReferenceId?, SkillData> getSkillData
    )
    {
        switch (spec.Metadata.Kind)
        {
            case ETemplateKind.Ref:
            {
                this.ResolveReference(spec, getCharacterData);
                return;
            }
            case ETemplateKind.Inline:
            {
                this.ResolveInline(spec, getStatData, getSkillData);
                return;
            }
            case ETemplateKind.Override:
            {
                this.ResolveOverride(spec, getCharacterData, getStatData, getSkillData);
                return;
            }
            case ETemplateKind.Instance:
            {
                this.ResolveInstance(spec, getSkillData);
                return;
            }
            default:
            {
                throw new InvalidOperationException("Invalid character spec kind");
            }
        }
    }

    private void ResolveReference(
        ReferenceSpec spec,
        Func<ReferenceId?, CharacterData> getCharacterData
    )
    {
        CharacterData characterData = getCharacterData(spec.Metadata.ReferenceId);

        this.Name = characterData.Name;
        this.Description = characterData.Description;
        this.Tags = characterData.Tags;
        this.Stats.AddRange(characterData.Stats);
        this.Skills.AddRange(characterData.Skills);
    }

    private void ResolveInline(
        ReferenceSpec spec,
        Func<ReferenceId?, StatData> getStatData,
        Func<ReferenceId?, SkillData> getSkillData
    )
    {
        var inlineSpec = spec as InlineSpec<CharacterTemplateSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a character template");
        }

        this.Name = inlineSpec.Template.Name;
        this.Description = inlineSpec.Template.Description;
        this.Tags = [.. inlineSpec.Template.Tags];
        this.Stats.AddRange(this.ResolveStats(inlineSpec.Template.Stats, getStatData));
        this.Skills.AddRange(this.ResolveSkills(inlineSpec.Template.Skills, getSkillData));
    }

    private void ResolveOverride(
        ReferenceSpec spec,
        Func<ReferenceId?, CharacterData> getCharacterData,
        Func<ReferenceId?, StatData> getStatData,
        Func<ReferenceId?, SkillData> getSkillData
    )
    {
        var overrideSpec = spec as OverrideSpec<CharacterTemplateSpec, CharacterOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a character template");
        }

        CharacterData characterData = getCharacterData(spec.Metadata.ReferenceId);

        this.Name = overrideSpec.Override.Name ?? this.Name;
        this.Description = overrideSpec.Override.Description ?? this.Description;
        this.Tags = overrideSpec.Override.Tags ?? this.Tags;

        if (overrideSpec.Override.Stats is not null)
        {
            this.Stats.AddRange(this.ResolveStats(overrideSpec.Override.Stats, getStatData));
        }
        else
        {
            this.Stats.AddRange(characterData.Stats);
        }

        if (overrideSpec.Override.Skills is not null)
        {
            this.Skills.AddRange(this.ResolveSkills(overrideSpec.Override.Skills, getSkillData));
        }
        else
        {
            this.Skills.AddRange(characterData.Skills);
        }
    }

    private void ResolveInstance(ReferenceSpec spec, Func<ReferenceId?, SkillData> getSkillData)
    {
        var instanceSpec = spec as InstanceSpec<CharacterTemplateSpec, CharacterData>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a character template");
        }

        this.Name = instanceSpec.Instance.Name;
        this.Description = instanceSpec.Instance.Description;
        this.Tags = [.. instanceSpec.Instance.Tags];
        this.Stats.AddRange(instanceSpec.Instance.Stats);
        this.Skills.AddRange(instanceSpec.Instance.Skills);
    }

    private List<StatData> ResolveStats(
        List<ReferenceSpec> statSpecs,
        Func<ReferenceId?, StatData> getStatData
    )
    {
        List<StatData> statDatas = new();
        foreach (var statSpec in statSpecs)
        {
            StatData statData = getStatData(statSpec.Metadata.ReferenceId);
            statDatas.Add(statData);
        }

        return statDatas;
    }

    private List<SkillData> ResolveSkills(
        List<ReferenceSpec> skillSpecs,
        Func<ReferenceId?, SkillData> getSkillData
    )
    {
        List<SkillData> skillDatas = new();
        foreach (var skillSpec in skillSpecs)
        {
            SkillData skillData = getSkillData(skillSpec.Metadata.ReferenceId);
            skillDatas.Add(skillData);
        }

        return skillDatas;
    }
}
