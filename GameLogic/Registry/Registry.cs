namespace GameLogic.Registry;

using System.Linq;
using System.Reflection.Metadata;
using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

public class EntityRegistry<T>
{
    private Dictionary<ReferenceId, T> entitiesById = new();

    public EntityRegistry() { }

    public bool TryAdd(T entity, ReferenceId referenceId)
    {
        return this.entitiesById.TryAdd(referenceId, entity);
    }

    public bool TryGet(ReferenceId referenceId, out T? entity)
    {
        return this.entitiesById.TryGetValue(referenceId, out entity);
    }
}

public class EntitiesRegistry
{
    public EntityRegistry<SkillTemplate> Skills { get; set; } = new();
}

public interface IRegistry
{
    public void Load(List<string> paths);

    public void Load(List<ReferenceSpec> records);

    /// <summary>
    /// Get a reference by its id. Should only be called during initialization
    /// so that the program fails fast if references are not setup correctly. Do
    /// not call this after initialization.
    /// </summary>
    /// <param name="referenceId">The id of the reference to get.</param>
    /// <returns>The reference.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the reference is not found.</exception>
    public IReference<object, ReferenceSpec> GetReference(ReferenceId? referenceId);

    /// <summary>
    /// Try to get a reference by its id. This should be called after initialization
    /// to get the reference value as it will fail gracefully if the reference is not found.
    /// </summary>
    /// <typeparam name="TReference">The type of the reference to get.</typeparam>
    /// <param name="referenceId">The id of the reference to get.</param>
    /// <param name="referenceValue">The reference value. Null if the reference is not found.</param>
    /// <returns>True if the reference is found, false otherwise.</returns>
    public bool TryGetReference<TReference>(
        ReferenceId? referenceId,
        out IReference<TReference, ReferenceSpec>? reference
    )
        where TReference : class;

    /// <summary>
    /// Try to get a reference by its id. This should be called after initialization
    /// to get the reference value as it will fail gracefully if the reference.
    /// Returns a generic reference that can be cast to the specific reference type.
    /// </summary>
    /// <param name="referenceId"></param>
    /// <param name="reference"></param>
    /// <returns></returns>
    public bool TryGetReference(
        ReferenceId? referenceId,
        out IReference<object, ReferenceSpec>? reference
    );

    public bool TryAdd(IReference<object, ReferenceSpec> reference, ReferenceId referenceId);
}

// TODO: Circular dep if I import entity since they use registry?
public class Registry : IRegistry
{
    private EntitiesRegistry entitiesRegistry = new();
    private Dictionary<ReferenceId, IReference<object, ReferenceSpec>> referencesById = new();

    public void Load(List<string> paths)
    {
        List<ReferenceSpec> records = new();
        foreach (string path in paths)
        {
            ReferenceSpec record = JsonConfigLoader.LoadFromFile<ReferenceSpec>(path);
            if (record is null)
            {
                throw new InvalidOperationException($"Failed to load record from {path}");
            }

            records.Add(record);
        }

        this.Load(records);
    }

    public void Load(List<ReferenceSpec> records)
    {
        // Dependencies can only be resolved after all references are created.
        // References can only be initialized (wrapped types created) after all
        //  dependencies are resolved.
        this.referencesById = records.ToDictionary(
            record => record.ReferenceMetadata.ReferenceId,
            record =>
                (IReference<object, ReferenceSpec>)
                    ReferenceFactory.CreateReferenceFromRecord(record, this.entitiesRegistry)
        );

        foreach (var (referenceId, reference) in this.referencesById)
        {
            reference.Resolve((IRegistry)this);
        }

        foreach (var (referenceId, reference) in this.referencesById)
        {
            reference.Initialize();
        }
    }

    public IReference<object, ReferenceSpec> GetReference(ReferenceId? referenceId)
    {
        if (referenceId is null)
        {
            throw new ArgumentNullException(nameof(referenceId));
        }
        ReferenceId id = referenceId.Value;

        this.referencesById.TryGetValue(id, out IReference<object, ReferenceSpec>? reference);
        if (reference is null)
        {
            throw new KeyNotFoundException($"Reference {referenceId} not found");
        }

        return reference;
    }

    public bool TryGetReference<TReference>(
        ReferenceId? referenceId,
        out IReference<TReference, ReferenceSpec>? reference
    )
        where TReference : class
    {
        this.TryGetReference(referenceId, out IReference<object, ReferenceSpec>? genericReference);
        reference = genericReference as IReference<TReference, ReferenceSpec>;
        return reference is not null;
    }

    public bool TryGetReference(
        ReferenceId? referenceId,
        out IReference<object, ReferenceSpec>? reference
    )
    {
        if (referenceId is null)
        {
            reference = null;
            return false;
        }
        ReferenceId id = referenceId.Value;

        this.referencesById.TryGetValue(id, out reference);
        return reference is not null;
    }

    public bool TryAdd(IReference<object, ReferenceSpec> reference, ReferenceId referenceId)
    {
        return this.referencesById.TryAdd(
            referenceId,
            (IReference<object, ReferenceSpec>)reference
        );
    }
}

// TODO: Consider adding method to stored references that tells the type of
// their underlying type so that we can avoid casting when getting references.
// Or allow for searching by type.
