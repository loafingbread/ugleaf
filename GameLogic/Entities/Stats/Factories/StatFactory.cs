namespace GameLogic.Entities.Stats.Factories;

using GameLogic.Entities.Stats.Capabilities;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

public static class StatFactory
{
    public static Stat CreateStatFromData(StatData data, BuildContext buildContext)
    {
        return new Stat(data.ReferenceId, StatFactory.CreateStatModelFromData(data, buildContext));
    }

    private static StatModel CreateStatModelFromData(StatData data, BuildContext buildContext)
    {
        IBounds? bounds = CapabilitiesFactory.CreateBoundsFromData(data.Capabilities.Bounds);
        IMax? max = CapabilitiesFactory.CreateMaxFromData(data.Capabilities.Max, buildContext);
        IValueModel valueModel = ValueModelFactory.FromSpecToValueModel(data.ValueModel);

        return new StatModel(valueModel, bounds, max);
    }
}
