namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Utils;

public class CharacterTemplate
    : ITemplate<CharacterOverrideRecord>,
        IDeepCopyable<CharacterTemplate>
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
    public CharacterOverrideRecord? TemplateOverride { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public StatBlock Stats { get; set; }
    public List<Skill> Skills { get; set; } = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CharacterTemplate(
        ReferenceUnionMetadata referenceMetadata,
        CharacterTemplateRecord? templateRecord,
        CharacterOverrideRecord? templateOverride
    )
    {
        if (referenceMetadata.Kind == EReferenceKind.Instance)
        {
            throw new InvalidOperationException("Templates cannot be loaded from instances");
        }
        else if (referenceMetadata.Kind == EReferenceKind.Inline && templateRecord is null)
        {
            throw new InvalidOperationException("Inline templates must have a template record");
        }
        else if (referenceMetadata.Kind == EReferenceKind.Override && templateOverride is null)
        {
            throw new InvalidOperationException("Override templates must have a template override");
        }
        else if (referenceMetadata.Kind != EReferenceKind.Ref)
        {
            throw new InvalidOperationException("Invalid reference kind");
        }

        this.ReferenceMetadata = referenceMetadata;
        this.TemplateOverride = templateOverride;
        this.ApplyTemplateRecord(templateRecord);
    }
#pragma warning restore CS8618

    private void ApplyTemplateRecord(CharacterTemplateRecord? templateRecord)
    {
        if (templateRecord is null)
        {
            return;
        }

        this.Name = templateRecord.Name;
        this.Description = templateRecord.Description;
        this.Tags = [.. templateRecord.Tags];
        this.Stats = StatFactory.CreateStatBlockFromRecord(templateRecord);
        this.Skills = this.CreateSkillsFromReferences(templateRecord.Skills);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CharacterTemplate(CharacterTemplate template)
    {
        this.ApplyTemplate(template);
    }
#pragma warning restore CS8618

    protected List<Skill> CreateSkillsFromReferences(List<ReferenceUnionSpec> skills)
    {
        return skills
            .Select(
                (ReferenceUnionSpec record) =>
                {
                    Reference<SkillTemplate, Skill> skillReference =
                        SkillFactory.CreateSkillReferenceFromRecord(record);
                    return skillReference.Template?.Instantiate();
                }
            )
            .Select(
                (Skill? skill) =>
                {
                    return skill is not null
                        ? skill
                        : throw new InvalidOperationException(
                            "Skill template should not be null while loading skill files"
                        );
                }
            )
            .ToList();
    }

    protected void ApplyTemplate(CharacterTemplate template)
    {
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Stats = template.Stats.DeepCopy();
        this.Skills = template.Skills.DeepCopyList();
    }

    public void LoadReferences(IRegistry registry)
    {
        if (this.ReferenceMetadata.Kind == EReferenceKind.Inline)
        {
            return;
        }

        registry.TryGetValue<Reference<CharacterTemplate, Character>>(
            this.ReferenceMetadata,
            out Reference<CharacterTemplate, Character> referenceValue
        );

        if (referenceValue.Template is null)
        {
            throw new InvalidOperationException("Template reference should not be null");
        }

        if (this.ReferenceMetadata.Kind == EReferenceKind.Ref)
        {
            this.ApplyTemplate(referenceValue.Template);
        }
        else if (this.ReferenceMetadata.Kind == EReferenceKind.Override)
        {
            this.ApplyTemplate(referenceValue.Template);
            this.ApplyOverrides(this.TemplateOverride);
        }
    }

    protected void ApplyOverrides(CharacterOverrideRecord? overrideRecord)
    {
        if (overrideRecord is null)
        {
            return;
        }

        this.TemplateOverride = overrideRecord;
        this.Name = overrideRecord.Name ?? this.Name;
        this.Description = overrideRecord.Description ?? this.Description;
        this.Tags = overrideRecord.Tags ?? this.Tags;
        this.Stats = overrideRecord.Stats is not null
            ? StatFactory.CreateStatBlockFromReferences(overrideRecord.Stats)
            : this.Stats;
        this.Skills = overrideRecord.Skills is not null
            ? this.CreateSkillsFromReferences(overrideRecord.Skills)
            : this.Skills;
    }

    public CharacterTemplate DeepCopy()
    {
        return new CharacterTemplate(this);
    }

    public Character Instantiate()
    {
        return CharacterFactory.CreateCharacterFromTemplate(this);
    }
}

public class Character : CharacterTemplate, IInstance<CharacterRecord>, IDeepCopyable<Character>
{
    public InstanceId InstanceId { get; set; }
    public CharacterRecord? InstanceState { get; set; }

    public Character(
        ReferenceUnionMetadata referenceMetadata,
        InstanceId instanceId,
        CharacterRecord? instanceState
    )
        : base(referenceMetadata, null, null)
    {
        if (referenceMetadata.Kind == EReferenceKind.Instance)
        {
            throw new InvalidOperationException("Character reference is not an instance");
        }

        this.InstanceId = instanceId;
        this.InstanceState = instanceState;
    }

    public Character(Character character)
        : base((character as CharacterTemplate).DeepCopy())
    {
        this.InstanceId = Ids.Instance();
    }

    public new void LoadReferences(IRegistry registry)
    {
        if (this.InstanceState is not null)
        {
            this.ApplyInstanceState();
            return;
        }

        registry.TryGetValue<Reference<CharacterTemplate, Character>>(
            this.ReferenceMetadata,
            out Reference<CharacterTemplate, Character> referenceValue
        );

        if (referenceValue.Template is null)
        {
            throw new InvalidOperationException("Template reference should not be null");
        }

        this.ApplyTemplate(referenceValue.Template);
    }

    private void ApplyInstanceState()
    {
        if (this.InstanceState is null)
        {
            return;
        }

        this.Name = this.InstanceState.Name;
        this.Description = this.InstanceState.Description;
        this.Tags = [.. this.InstanceState.Tags];
        this.Stats = StatFactory.CreateStatBlockFromRecord(this.InstanceState);
        this.Skills = this.CreateSkillsFromReferences(this.InstanceState.Skills);
    }

    public new Character DeepCopy()
    {
        return new Character(this);
    }
}

public sealed class CharacterReference : Reference<Character>
{
    public CharacterReference(ReferenceUnionMetadata metadata, Character? value)
        : base(metadata, value) { }
}

public sealed class CharacterTemplateReference : Reference<CharacterTemplate>
{
    public CharacterTemplateReference(ReferenceUnionMetadata metadata, CharacterTemplate? value)
        : base(metadata, value) { }
}
