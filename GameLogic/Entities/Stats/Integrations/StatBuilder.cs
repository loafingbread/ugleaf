namespace GameLogic.Entities.Stats.Integrations;

using GameLogic.Entities.Stats.Stat;

public class StatBuilder 
{
    public Stat Build(ReferenceId referenceId, IStatModel model)
    {
        return new Stat(referenceId, model);
    }
}