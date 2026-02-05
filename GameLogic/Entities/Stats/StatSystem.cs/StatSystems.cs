namespace GameLogic.Entities.Stats;

public static class StatBoundsSystem
{
    public static float ApplyBounds(Stat stat, float value)
    {
        float newValue = StatBoundsSystem.ApplyBoundsToValue(stat, value);
        newValue = StatBoundsSystem.ApplyMaxToValue(stat, newValue);

        return newValue;
    }

    public static float ApplyBoundsToValue(Stat stat, float value)
    {
        if (stat.Model.Bounds is null)
        {
            return value;
        }

        return stat.Model.Bounds.ApplyBounds(value);
    }

    public static float ApplyMaxToValue(Stat stat, float value)
    {
        if (stat.Model.Max is null)
        {
            return value;
        }

        return stat.Model.Max.ApplyMaxStat(value);
    }
}

public static class StatMutableValueSystem
{
    public static float GetValue(Stat stat)
    {
        if (stat.Model.MutableValue is null)
        {
            throw new InvalidOperationException("Stat model is not mutable");
        }

        return stat.Model.MutableValue.Value;
    }

    public static float Add(Stat stat, float delta)
    {
        if (stat.Model.MutableValue is null)
        {
            throw new InvalidOperationException("Stat model is not mutable");
        }

        return stat.Model.MutableValue.Add(delta);
    }

    public static float Set(Stat stat, float value)
    {
        if (stat.Model.MutableValue is null)
        {
            throw new InvalidOperationException("Stat model is not mutable");
        }

        return stat.Model.MutableValue.Set(value);
    }
}

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
