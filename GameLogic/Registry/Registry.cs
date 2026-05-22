namespace GameLogic.Registry;

public class Registry<T>
{
    private readonly Dictionary<ReferenceId, T> _entries = new();

    public bool TryAdd(ReferenceId id, T value) => _entries.TryAdd(id, value);

    public bool TryGet(ReferenceId id, out T? value) => _entries.TryGetValue(id, out value);

    public T Get(ReferenceId id) =>
        _entries.TryGetValue(id, out T? value)
            ? value
            : throw new KeyNotFoundException($"No entry found for id '{id}'");

    public bool Contains(ReferenceId id) => _entries.ContainsKey(id);

    public IReadOnlyDictionary<ReferenceId, T> All => _entries;
}
