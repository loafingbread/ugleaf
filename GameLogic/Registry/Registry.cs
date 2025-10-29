namespace GameLogic.Registry;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

public interface IRegistry
{
    void Load(List<ReferenceUnionSpec> records);

    bool TryGetValue<T>(ReferenceUnionMetadata referenceMetadata, out T referenceValue)
        where T : class;
}

// TODO: Circular dep if I import entity since they use registry?
public class Registry
{
    private List<Reference<CharacterTemplate, Character>> characters = new();
    private List<Reference<SkillTemplate, Skill>> skills = new();
    private List<Reference<UsableTemplate, Usable>> usables = new();
    private List<Reference<EffectTemplate, IEffect>> effects = new();
    private List<Reference<Stat, Stat>> stats = new();

    void Load(List<ReferenceUnionSpec> records)
    {
        foreach (ReferenceUnionSpec record in records)
        {
            switch (record.Metadata.TemplateType)
            {
                case ETemplateType.Character:
                    this.characters.Add(
                        CharacterFactory.CreateCharacterReferenceFromRecord(record)
                    );
                    break;
                // case ETemplateType.Item:
                case ETemplateType.Skill:
                    this.skills.Add(SkillFactory.CreateSkillReferenceFromRecord(record));
                    break;
                case ETemplateType.Usable:
                    this.usables.Add(UsableFactory.CreateUsableReferenceFromRecord(record));
                    break;
                case ETemplateType.Effect:
                    this.effects.Add(EffectFactory.CreateEffectReferenceFromRecord(record));
                    break;
                // TODO: How to implement statblock references on top of stat reference?
                // Custom logic?
                case ETemplateType.Stat:
                    this.stats.Add(StatFactory.CreateStatReferenceFromRecord(record));
                    break;
                default:
                    throw new NotImplementedException("Reference type not implemented for loading");
            }
        }
    }

    bool TryGetValue<T>(ReferenceUnionMetadata referenceMetadata, out T? referenceValue)
        where T : class
    {
        switch (referenceMetadata.TemplateType)
        {
            case ETemplateType.Character:
                referenceValue =
                    this.TryGetReference<CharacterTemplate, Character>(
                        this.characters,
                        referenceMetadata
                    ) as T;
                break;
            // case ETemplateType.Item:
            case ETemplateType.Skill:
                referenceValue =
                    this.TryGetReference<SkillTemplate, Skill>(this.skills, referenceMetadata) as T;
                break;
            case ETemplateType.Usable:
                referenceValue =
                    this.TryGetReference<UsableTemplate, Usable>(this.usables, referenceMetadata)
                    as T;
                break;
            case ETemplateType.Effect:
                referenceValue =
                    this.TryGetReference<EffectTemplate, IEffect>(this.effects, referenceMetadata)
                    as T;
                break;
            case ETemplateType.Stat:
                referenceValue =
                    this.TryGetReference<Stat, Stat>(this.stats, referenceMetadata) as T;
                break;
            default:
                throw new NotImplementedException("Reference type is not implemented for getting");
        }

        return referenceValue is not null;
    }

    Reference<TTemplate, TInstance>? TryGetReference<TTemplate, TInstance>(
        List<Reference<TTemplate, TInstance>> references,
        ReferenceUnionMetadata referenceMetadata
    )
        where TTemplate : class
        where TInstance : class
    {
        foreach (Reference<TTemplate, TInstance> reference in references)
        {
            if (reference.ReferenceMetadata.TemplateId == referenceMetadata.TemplateId)
            {
                return reference;
            }
        }

        return null;
    }
}

// public class TemplateRegistry : IRegistry
// {
//     private readonly Dictionary<TemplateIdentifier, object> _characters = new();
//     private readonly Dictionary<TemplateIdentifier, object> _skills = new();
//     private readonly Dictionary<TemplateIdentifier, object> _usables = new();
//     private readonly Dictionary<TemplateIdentifier, object> _effects = new();
// }

// TODO: How do I store all the different types of references in a list? Or do I
// separate by type? I think I separate by type
public class Reference<TTemplate, TInstance>
    where TTemplate : class
    where TInstance : class
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
    public TTemplate? Template { get; set; }
    public TInstance? Instance { get; set; }

    public Reference(
        ReferenceUnionMetadata referenceMetadata,
        TTemplate? template,
        TInstance? instance
    )
    {
        this.ReferenceMetadata = referenceMetadata;

        this.Template = template;
        this.Instance = instance;
    }

    public TTemplate ResolveTemplate(IRegistry registry) =>
        registry.TryGetValue<TTemplate>(ReferenceMetadata, out var referenceValue)
            ? referenceValue
            : throw new KeyNotFoundException(
                $"Missing reference {ReferenceMetadata} of type {typeof(TTemplate).Name}"
            );

    public TInstance ResolveInstance(IRegistry registry) =>
        registry.TryGetValue<TInstance>(ReferenceMetadata, out var referenceValue)
            ? referenceValue
            : throw new KeyNotFoundException(
                $"Missing reference {ReferenceMetadata} of type {typeof(TInstance).Name}"
            );
}
