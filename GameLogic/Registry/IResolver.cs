namespace GameLogic.Registry;

/// <summary>
/// Converts a raw deserialized spec into fully resolved data.
/// Implementations handle ref lookups, patch application, and flattening.
/// Dependencies are injected into the concrete resolver via constructor.
/// </summary>
public interface IResolver<TSpec, TData>
{
    TData Resolve(TSpec spec);
}
