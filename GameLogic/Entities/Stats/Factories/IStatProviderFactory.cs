namespace GameLogic.Entities.Stats.Factories;

using GameLogic.Entities.Stats.Query;
using GameLogic.Registry;

public interface IStatProviderFactory
{
    IStatProvider Create();
}
