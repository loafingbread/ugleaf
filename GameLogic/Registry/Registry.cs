namespace GameLogic.Registry;

using System.Linq;
using GameLogic.Config;
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

    bool TryGetReference<TReference>(
        ReferenceId referenceId,
        ETemplateType templateType,
        out TReference? referenceValue
    )
        where TReference : class;
}

// TODO: Circular dep if I import entity since they use registry?
public class Registry
{
    private List<CharacterTemplateReference> characterTemplates = new();
    private List<SkillTemplateReference> skillTemplates = new();
    private List<UsableTemplateReference> usableTemplates = new();
    private List<EffectTemplateReference> effectTemplates = new();
    private List<StatReference> stats = new();

    public void Load(List<string> paths)
    {
        List<ReferenceUnionSpec> records = new();
        foreach (string path in paths)
        {
            ReferenceUnionSpec record = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(path);
            if (record is null)
            {
                throw new InvalidOperationException($"Failed to load record from {path}");
            }

            records.Add(record);
        }

        this.Load(records);
    }

    public void Load(List<ReferenceUnionSpec> records)
    {
        foreach (ReferenceUnionSpec record in records)
        {
            if (record.Metadata.Kind == EReferenceKind.Instance)
            {
                continue;
            }
            else if (record.Metadata.Kind == EReferenceKind.Ref)
            {
                continue;
            }

            switch (record.Metadata.TemplateType)
            {
                case ETemplateType.Character:
                    this.characterTemplates.Add(
                        CharacterFactory.CreateCharacterReferenceFromRecord(record)
                    );
                    break;
                // case ETemplateType.Item:
                case ETemplateType.Skill:
                    this.skillTemplates.Add(SkillFactory.CreateSkillReferenceFromRecord(record));
                    break;
                case ETemplateType.Usable:
                    this.usableTemplates.Add(UsableFactory.CreateUsableReferenceFromRecord(record));
                    break;
                case ETemplateType.Effect:
                    this.effectTemplates.Add(EffectFactory.CreateEffectReferenceFromRecord(record));
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

    public bool TryGetReference(
        ReferenceId referenceId,
        ETemplateType templateType,
        out IReference? referenceValue
    )
    {
        switch (templateType)
        {
            case ETemplateType.Character:
                referenceValue =
                    this.TryGetReferenceFromList(this.characterTemplates, referenceId, templateType)
                    as CharacterTemplateReference;
                break;
            // case ETemplateType.Item:
            case ETemplateType.Skill:
                referenceValue =
                    this.TryGetReferenceFromList(this.skillTemplates, referenceId, templateType)
                    as SkillTemplateReference;
                break;
            case ETemplateType.Usable:
                referenceValue =
                    this.TryGetReferenceFromList(this.usableTemplates, referenceId, templateType)
                    as UsableTemplateReference;
                break;
            case ETemplateType.Effect:
                referenceValue =
                    this.TryGetReferenceFromList(this.effectTemplates, referenceId, templateType)
                    as EffectTemplateReference;
                break;
            case ETemplateType.Stat:
                referenceValue =
                    this.TryGetReferenceFromList(this.stats, referenceId, templateType)
                    as StatReference;
                break;
            default:
                throw new NotImplementedException("Reference type is not implemented for getting");
        }

        return referenceValue is not null;
    }

    private IReference? TryGetReferenceFromList(
        IEnumerable<IReference> references,
        ReferenceId referenceId,
        ETemplateType templateType
    )
    {
        foreach (IReference reference in references)
        {
            if (
                reference.Metadata.ReferenceId == referenceId
                && reference.Metadata.TemplateType == templateType
            )
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

public interface IReference
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }

    /// <summary>
    /// Resolve the reference using the registry recursively. 
    /// This will set the value of the reference.
    /// Should throw an exception if the reference is not found.
    /// </summary>
    /// <param name="registry"></param>
    public void Resolve(IRegistry registry);
}

public class Reference<T> : IReference
    where T : class
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
    public T? Value { get; set; }

    public Reference(ReferenceUnionMetadata referenceMetadata, T? value)
    {
        this.ReferenceMetadata = referenceMetadata;

        this.Value = value;
    }

    public T GetValue() =>
        this.Value
        ?? throw new InvalidOperationException(
            "Reference value is null. Should have been resolved before getting value."
        );

    public Reference<T>? GetDependency()
    {
        if (this.ReferenceMetadata.TemplateId.value is null)
        {
            return null;
        }

        return new Reference<T>(this.ReferenceMetadata, this.Value);
    }

    public void Resolve(IRegistry registry)
    {
        if (this.ReferenceMetadata.DependencyId is null)
        {
            return;
        }

        this.Value = registry.TryGetValue<T>(this.ReferenceMetadata, out var referenceValue)
            ? referenceValue
            : throw new KeyNotFoundException(
                $"Missing reference {this.ReferenceMetadata} of type {typeof(T).Name}"
            );
    }
}
