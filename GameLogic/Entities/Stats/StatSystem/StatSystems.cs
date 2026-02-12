namespace GameLogic.Entities.Stats.StatSystem;

using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

public static class StatSystem
{
    /// <summary>
    /// Adds a change in value to the base value of the stat.
    /// </summary>
    /// <param name="stat">The stat to add the base value to.</param>
    /// <param name="delta">The value to add.</param>
    /// <param name="newValue">The new base value of the stat.</param>
    /// <returns>True if the base value was added successfully, false otherwise.</returns>
    public static bool Add(Stat stat, float delta, out float newValue)
    {
        return StatSystem.Set(stat, stat.BaseValue + delta, out newValue);
    }

    /// <summary>
    /// Sets the base value of the stat to the given value.
    /// </summary>
    /// <param name="stat">The stat to set the base value of.</param>
    /// <param name="value">The value to set the base value to.</param>
    /// <param name="newValue">The new base value of the stat.</param>
    /// <returns>True if the value was set successfully, false otherwise.</returns>
    public static bool Set(Stat stat, float value, out float newValue)
    {
        MutableValueModel? mutableValueModel = stat.Model.ValueModel as MutableValueModel;
        if (mutableValueModel is null)
        {
            newValue = float.NaN;
            return false;
        }

        newValue = StatValueSystem.Set(stat, value);
        newValue = StatBoundsSystem.ApplyBounds(stat, value);

        return true;
    }

    /// <summary>
    /// Calculates the value of the stat using the given formula.
    /// NOTE: Does not set value of stat since it is not a mutable value.
    /// It calculates the formula value and applies bounds to it.
    /// </summary>
    /// <param name="stat">The stat to calculate the value of.</param>
    /// <param name="getValueFunc">The function to get the value of the stat.</param>
    /// <param name="newValue">The new value of the stat.</param>
    /// <returns>True if the value was calculated successfully, false otherwise.</returns>
    public static bool Calculate(
        Stat stat,
        Func<ReferenceId?, bool, float> getValueFunc,
        out float newValue
    )
    {
        FormulaValueModel? formulaValueModel = stat.Model.ValueModel as FormulaValueModel;
        if (formulaValueModel is null)
        {
            newValue = float.NaN;
            return false;
        }

        newValue = formulaValueModel.CalculateValue(getValueFunc);
        newValue = StatBoundsSystem.ApplyBounds(stat, newValue);

        return true;
    }

    /// <summary>
    /// Gets base value of the stat with no modifiers applied.
    /// </summary>
    /// <param name="stat">The stat to get the base value of.</param>
    /// <returns>The base value of the stat.</returns>
    public static float GetBaseValue(Stat stat)
    {
        return stat.BaseValue;
    }

    /// <summary>
    /// Gets current value of the stat with modifiers applied.
    /// </summary>
    /// <param name="stat">The stat to get the current value of.</param>
    /// <returns>The current value of the stat.</returns>
    public static float GetValue(Stat stat)
    {
        return stat.Value;
    }
}
