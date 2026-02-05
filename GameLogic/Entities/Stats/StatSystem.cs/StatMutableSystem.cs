namespace GameLogic.Entities.Stats.StatSystem;

using GameLogic.Entities.Stats.Stat;

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
