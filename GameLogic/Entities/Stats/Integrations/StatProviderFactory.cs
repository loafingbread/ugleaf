namespace GameLogic.Entities.Stats.Integrations;

using GameLogic.Entities.Stats.Query;
using GameLogic.Registry;

public interface IStatProviderFactory
{
    IStatProvider Create();
}

public class StatProviderFactory : IStatProviderFactory
{
    private IRegistry registry { get; init; }

    public StatProviderFactory(IRegistry registry)
    {
        this.registry = registry;
    }

    public IStatProvider Create()
    {
        return new StatProvider(this.registry);
    }
}
