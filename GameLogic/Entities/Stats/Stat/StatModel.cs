namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Entities.Stats.Capabilities;

public interface IStatModel
{
    public IBounds? Bounds { get; }
    public IMax? Max { get; }
    public IMutable? MutableValue { get; }
    public IImmutable? ImmutableValue { get; }

    public float GetValue();
}

public class StatModel : IStatModel
{
    public IBounds? Bounds { get; init; }
    public IMax? Max { get; init; }
    public IMutable? MutableValue { get; init; }
    public IImmutable? ImmutableValue { get; init; }

    public StatModel(IBounds? bounds, IMax? max, IMutable? mutableValue, IImmutable? immutableValue)
    {
        if (mutableValue is not null && immutableValue is not null)
        {
            throw new InvalidOperationException("Cannot have both mutable and immutable value");
        }

        this.Bounds = bounds;
        this.Max = max;
        this.MutableValue = mutableValue;
        this.ImmutableValue = immutableValue;
    }

    public float GetValue()
    {
        float value = 0f;
        if (this.MutableValue is not null)
        {
            value = this.MutableValue.Value;
        }
        else if (this.ImmutableValue is not null)
        {
            value = this.ImmutableValue.CalculateValue();
        }
        else
        {
            throw new InvalidOperationException("No value or formula provided");
        }

        value = this.Bounds?.ApplyBounds(value) ?? value;
        value = this.Max?.ApplyMaxStat(value) ?? value;
        return value;
    }
}
