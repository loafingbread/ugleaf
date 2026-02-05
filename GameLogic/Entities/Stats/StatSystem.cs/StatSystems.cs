namespace GameLogic.Entities.Stats.StatSystem;

using GameLogic.Entities.Stats.Stat;

public static class StatSystem
{
    public static bool Add(Stat stat, float delta, out float newValue)
    {
        if (stat.Model.MutableValue is null)
        {
            newValue = float.NaN;
            return false;
        }

        newValue = StatMutableValueSystem.Add(stat, delta);
        newValue = StatBoundsSystem.ApplyBounds(stat, newValue);
        newValue = StatMutableValueSystem.Set(stat, newValue);

        return true;
    }

    public static bool Set(Stat stat, float value, out float newValue)
    {
        if (stat.Model.MutableValue is null)
        {
            newValue = float.NaN;
            return false;
        }

        newValue = StatBoundsSystem.ApplyBounds(stat, value);
        newValue = StatMutableValueSystem.Set(stat, newValue);

        return true;
    }

    public static bool Calculate(Stat stat, out float newValue)
    {
        if (stat.Model.ImmutableValue is null)
        {
            newValue = float.NaN;
            return false;
        }

        newValue = stat.Model.ImmutableValue.CalculateValue();
        return true;
    }

    /// <summary>
    /// Gets base value of the stat with no modifiers applied.
    /// </summary>
    /// <param name="stat">The stat to get the base value of.</param>
    /// <returns>The base value of the stat.</returns>
    public static float GetBaseValue(Stat stat)
    {
        return stat.Value;
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
