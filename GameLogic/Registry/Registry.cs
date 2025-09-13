namespace GameLogic.Registry;

public interface IRegistry
{
    bool TryGetValue<T>(ReferenceUnionMetadata referenceMetadata, out T referenceValue)
        where T : class;
}

// public class TemplateRegistry : IRegistry
// {
//     private readonly Dictionary<TemplateIdentifier, object> _characters = new();
//     private readonly Dictionary<TemplateIdentifier, object> _skills = new();
//     private readonly Dictionary<TemplateIdentifier, object> _usables = new();
//     private readonly Dictionary<TemplateIdentifier, object> _effects = new();
// }

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
