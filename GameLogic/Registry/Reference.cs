namespace GameLogic.Registry;

using GameLogic.Entities.Skills;

public interface IRegistryReference
{
    /// <summary>
    /// The metadata of the reference in the registry.
    /// </summary>
    ReferenceUnionMetadata Metadata { get; }

    /// <summary>
    /// Whether the reference is resolved.
    /// </summary>
    bool IsResolved { get; set; }

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

public interface IRegistryReference<out T> : IRegistryReference
{
    /// <summary>
    /// Get the value of the reference. Should only be called after initializing the reference.
    /// </summary>
    /// <returns>The value of the reference.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the reference is not initialized.</exception>
    T GetValue();
}

public abstract class RegistryReferenceBase<T> : IRegistryReference<T>
{
    public ReferenceUnionMetadata Metadata { get; }
    public bool IsResolved { get; set; } = false;

    protected ReferenceSpec Spec { get; }

    protected T? Value { get; set; } = default(T);

    protected RegistryReferenceBase(ReferenceSpec spec)
    {
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

    protected abstract void ResolveDependencies(IRegistry registry);

    public abstract void Initialize();

    public virtual T GetValue() =>
        this.Value ?? throw new InvalidOperationException("Reference is not initialized");
}

public class SkillTemplateSpec : RegistryReferenceBase<SkillTemplate>
{
    private SkillTemplateSpec? templateRecord { get; set; } = null;
    private SkillOverrideSpec? overrideRecord { get; set; } = null;
    private SkillTemplate? value { get; set; } = null;

    public SkillTemplateSpec(ReferenceSpec referenceSpec)
        : base(referenceSpec) { }

    public override void ResolveDependencies(IRegistry registry)
    {
        if (this.Spec.Metadata.Kind == EReferenceKind.Inline)
        {
            var inlineSpec = this.Spec as InlineSpec<SkillTemplateSpec>;
            if (inlineSpec is null)
            {
                throw new InvalidOperationException("Inline spec is not a skill template");
            }

            this.templateRecord = inlineSpec.Template;
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

public class SkillInstanceReference : RegistryReferenceBase<Skill>
{
    private SkillInstanceSpec? instanceRecord { get; set; } = null;
    private Skill? value { get; set; } = null;

    public SkillInstanceReference(ReferenceSpec spec)
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
    public static IRegistryReference CreateReferenceFromRecord(ReferenceSpec record)
    {
        switch (record.Metadata.TemplateType)
        {
            case ETemplateType.Skill:
                return CreateSkillInstanceSpec(record);
            default:
                throw new InvalidOperationException("Invalid template type");
        }
    }

    private static IRegistryReference CreateSkillInstanceSpec(ReferenceSpec record)
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
            case EReferenceKind.Inline:
            case EReferenceKind.Override:
                return new SkillTemplateSpec(record);
            case EReferenceKind.Instance:
                return new SkillInstanceReference(record);
            default:
                throw new InvalidOperationException("Invalid skill reference kind");
        }
    }
}
