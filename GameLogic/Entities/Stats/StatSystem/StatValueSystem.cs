namespace GameLogic.Entities.Stats.StatSystem;

using GameLogic.Entities.Stats.Stat;

public static class StatValueSystem
{
    public static float GetValue(Stat stat)
    {
        return stat.Model.ValueModel.GetValue();
    }

    /// <summary>
    /// Adds a change in value to the base value of the stat. 
    /// </summary>
    /// <param name="stat">The stat to add the value to.</param>
    /// <param name="delta">The value to add.</param>
    /// <returns>The new base value of the stat.</returns>
    public static float Add(Stat stat, float delta)
    {
        return StatValueSystem.Set(stat, stat.BaseValue + delta);
    }

    /// <summary>
    /// Sets the base value of the stat to the given value.
    /// </summary>
    /// <param name="stat">The stat to set the value of.</param>
    /// <param name="value">The value to set the base value to.</param>
    /// <returns>The new base value of the stat.</returns>
    public static float Set(Stat stat, float value)
    {
        MutableValueModel? mutableValueModel = stat.Model.ValueModel as MutableValueModel;
        if (mutableValueModel is null)
        {
            System.Console.WriteLine("Warning: Stat model is not mutable");
            return float.NaN;
        }

        mutableValueModel.SetValue(value);
        return stat.BaseValue;
    }
}
