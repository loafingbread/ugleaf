namespace GameLogic.Registry;

/// <summary>
/// Creates a runtime template from resolved data.
/// Called once per entry after all specs have been resolved.
/// </summary>
public interface ITemplateFactory<TData, TTemplate>
{
    TTemplate Create(ReferenceId id, TData data);
}
