namespace GameLogic.Registry;

using GameLogic.Entities.Skills;

public interface IReference
{
    /// <summary>
    /// The metadata of the reference in the registry.
    /// </summary>
    ReferenceMetadata Metadata { get; }

    /// <summary>
    /// Whether the reference is resolved.
    /// </summary>
    bool IsResolved { get; set; }

    /// <summary>
    /// Resolve the reference by resolving dependencies.
    /// </summary>
    /// <param name="registry">The registry to resolve dependencies from.</param>
    void Resolve(IRegistry registry);

    /// <summary>
    /// Resolve dependencies of the reference by fetching them from the registry
    /// recursively. After resolving dependencies, the reference should have all
    /// dependencies needed to be instantiated.
    /// </summary>
    /// <param name="registry">The registry to fetch dependencies from.</param>
    /// <exception cref="InvalidOperationException">Thrown if dependencies are not found in the registry.</exception>
    void ResolveDependencies(IRegistry registry);

    /// <summary>
    /// Initialize the reference by initializing the wrapped type.
    /// Should only be called after resolving dependencies.
    /// <exception cref="InvalidOperationException">Thrown if dependencies are not resolved.</exception>
    /// </summary>
    void Initialize();
}

public interface IReference<out TValue, out TSpec> : IReference
    where TSpec : ReferenceSpec
{
    /// <summary>
    /// Get the value of the reference. Should only be called after initializing the reference.
    /// </summary>
    /// <returns>The value of the reference.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the reference is not initialized.</exception>
    TValue GetValue();

    /// <summary>
    /// Get the spec of the reference. Should only be called after resolving dependencies.
    /// </summary>
    /// <returns>The spec of the reference.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the reference is not resolved.</exception>
    TSpec GetSpec();
}

// TODO: Add record to store all deps in generic type TDeps
public abstract class ReferenceBase<TValue, TSpec> : IReference<TValue, TSpec>
    where TSpec : ReferenceSpec
{
    private EntityRegistry<TValue> entityRegistry { get; init; }
    public ReferenceMetadata Metadata { get; }
    public bool IsResolved { get; set; } = false;

    protected TSpec Spec { get; }

    protected TValue? Value { get; set; } = default(TValue);

    protected ReferenceBase(EntityRegistry<TValue> entityRegistry, TSpec spec)
    {
        this.entityRegistry = entityRegistry;
        this.Metadata = spec.Metadata;
        this.Spec = spec;

        if (this.Metadata.Kind == EReferenceKind.Inline)
        {
            this.IsResolved = true;
        }
    }

    public void Resolve(IRegistry registry)
    {
        if (this.IsResolved)
        {
            return;
        }

        this.ResolveDependencies(registry);
    }

    public abstract void ResolveDependencies(IRegistry registry);

    public abstract void Initialize();

    public virtual TValue GetValue()
    {
        this.entityRegistry.TryGet(this.Metadata.ReferenceId, out TValue? value);

        return value
            ?? throw new InvalidOperationException(
                "Reference is not initialized. Should not have been called before resolving dependencies."
            );
    }

    public virtual TSpec GetSpec() => this.Spec;
}
