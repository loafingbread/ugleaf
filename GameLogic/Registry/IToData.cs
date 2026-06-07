namespace GameLogic.Registry;

/// <summary>
/// Implemented by templates and instances to serialize back to their data form.
/// Used when writing changes to disk (save files, config edits).
/// </summary>
public interface IToData<TData>
{
    TData ToData();
}
