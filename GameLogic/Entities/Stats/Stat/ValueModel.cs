namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

public record ValueModelData
{
    // The type of value model to create
    public required EValueModel Type { get; init; }

    // The value to use if the type is fixed or mutable
    public float? Value { get; init; }

    // The formula to use if the type is formula
    public FormulaData? Formula { get; init; }
}

public enum EValueModel
{
    // Value is mutable and can be changed by the player
    Mutable,

    // Value is derived and controlled by a formula
    Formula,

    // Value is fixed and cannot be changed
    Fixed,
}

public interface IValueModel
{
    float GetValue();
}

public class MutableValueModel : IValueModel
{
    private float value { get; set; }

    public MutableValueModel(float value)
    {
        this.value = value;
    }

    public float GetValue()
    {
        return this.value;
    }

    public float SetValue(float value)
    {
        this.value = value;
        return this.value;
    }

    public float AddValue(float delta)
    {
        this.value = this.value + delta;
        return this.value;
    }
}

public class FormulaValueModel : IValueModel
{
    private Formula _formula { get; init; }

    public FormulaValueModel(Formula formula)
    {
        this._formula = formula;
    }

    public float GetValue()
    {
        return 0f;
    }

    public float CalculateValue(Func<ReferenceId?, bool, float> getValueFunc)
    {
        return this._formula.CalculateValue(getValueFunc);
    }
}

public class FixedValueModel : IValueModel
{
    private float value { get; init; }

    public FixedValueModel(float value)
    {
        this.value = value;
    }

    public float GetValue()
    {
        return this.value;
    }
}
