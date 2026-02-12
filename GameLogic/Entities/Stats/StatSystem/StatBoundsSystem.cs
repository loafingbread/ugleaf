namespace GameLogic.Entities.Stats.StatSystem;

using GameLogic.Entities.Stats.Stat;

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
