namespace GameLogic.Registry;

public interface IRegistry
{
    bool TryGetTemplate<T>(TemplateId id, out T template)
        where T : class, ITemplate;
}

public readonly record struct Ref<T>(TemplateId id)
    where T : class, ITemplate
{
    public T Resolve(IRegistry registry) =>
        registry.TryGetTemplate<T>(id, out var template)
            ? template
            : throw new KeyNotFoundException($"Missing template {id} of type {typeof(T).Name}");
}

public class TemplateRegistry : IRegistry
{
    private readonly Dictionary<TemplateId, object> _characters = new();
    private readonly Dictionary<TemplateId, object> _skills = new();
    private readonly Dictionary<TemplateId, object> _usables = new();
    private readonly Dictionary<TemplateId, object> _effects = new();
}