namespace GameLogic.Registry;

public interface IRegistry
{
    bool TryGetTemplate<T>(TemplateIdentifier templateIdentifier, out T template)
        where T : class, ITemplate;
}

public readonly record struct Ref<T>(TemplateIdentifier templateIdentifier)
    where T : class, ITemplate
{
    public T Resolve(IRegistry registry) =>
        registry.TryGetTemplate<T>(templateIdentifier, out var template)
            ? template
            : throw new KeyNotFoundException($"Missing template {templateIdentifier} of type {typeof(T).Name}");
}

// public class TemplateRegistry : IRegistry
// {
//     private readonly Dictionary<TemplateIdentifier, object> _characters = new();
//     private readonly Dictionary<TemplateIdentifier, object> _skills = new();
//     private readonly Dictionary<TemplateIdentifier, object> _usables = new();
//     private readonly Dictionary<TemplateIdentifier, object> _effects = new();
// }