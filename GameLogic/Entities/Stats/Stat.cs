namespace GameLogic.Entities.Stats;

using System.ComponentModel;
using GameLogic.Registry;
using GameLogic.Utils;

// TODO: Implement stat template vs stat value/instance

public class Stat
{
    public ReferenceId ReferenceId { get; set; }
    public IStatModel Model { get; set; }
    public StatModifiers Modifiers { get; set; } = new();
    public float BaseValue => this.Model.GetValue();
    public float Value => this.Modifiers.GetModifiedValueFromBase(this.ReferenceId, this.BaseValue);

    public Stat(ReferenceId referenceId, IStatModel model)
    {
        this.ReferenceId = referenceId;
        this.Model = model;
    }

    public bool AddModifier(StatModifier modifier)
    {
        return this.Modifiers.AddModifier(modifier);
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        return this.Modifiers.RemoveModifier(modifier);
    }
}

public interface IStatModel
{
    public IBounds? Bounds { get; }
    public IMaxStat? Max { get; }
    public IMutable? MutableValue { get; }
    public IImmutable? ImmutableValue { get; }

    public float GetValue();
}

public class StatModel : IStatModel
{
    public IBounds? Bounds { get; init; }
    public IMaxStat? Max { get; init; }
    public IMutable? MutableValue { get; init; }
    public IImmutable? ImmutableValue { get; init; }

    public StatModel(
        IBounds? bounds,
        IMaxStat? max,
        IMutable? mutableValue,
        IImmutable? immutableValue
    )
    {
        this.Bounds = bounds;

        if (mutableValue is not null && immutableValue is not null)
        {
            throw new InvalidOperationException("Cannot have both mutable and immutable value");
        }
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
