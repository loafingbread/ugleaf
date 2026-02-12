namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Entities.Stats.Capabilities;

public interface IStatModel
{
    public IValueModel ValueModel { get; }
    public IBounds? Bounds { get; }
    public IMax? Max { get; }

    public float GetValue();
}

public class StatModel : IStatModel
{
    public IValueModel ValueModel { get; init; }
    public IBounds? Bounds { get; init; }
    public IMax? Max { get; init; }

    public StatModel(IValueModel valueModel, IBounds? bounds, IMax? max)
    {
        this.ValueModel = valueModel;
        this.Bounds = bounds;
        this.Max = max;
    }

    public float GetValue()
    {
        float value = this.ValueModel.GetValue();

        value = this.Bounds?.ApplyBounds(value) ?? value;
        value = this.Max?.ApplyMaxStat(value) ?? value;
        return value;
    }
}
