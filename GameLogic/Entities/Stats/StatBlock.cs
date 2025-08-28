namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;

public class StatBlock
{
    public List<Stat> Stats { get; private set; } = new();
    public StatModifiers Modifiers { get; private set; } = new();

    public StatBlock(StatBlockRecord record)
    {
        foreach (StatRecord statRecord in record.Stats)
        {
            this.Stats.Add(StatFactory.CreateStatFromRecord(statRecord));
        }
    }

    public void AddModifier(StatModifier modifier)
    {
        this.Modifiers.AddModifier(modifier);
    }

    /// <summary>
    /// Removes a modifier from the stat block.
    /// </summary>
    /// <param name="modifier">The modifier to remove.</param>
    public void RemoveModifier(StatModifier modifier)
    {
        this.Modifiers.RemoveModifier(modifier);
    }

    /// <summary>
    /// Gets a stat from the stat block by name and type. If type is Any, any stat with the given name will be returned.
    /// </summary>
    /// <param name="name">The name of the stat to get.</param>
    /// <param name="type">The type of the stat to get. If Any, any stat with the given name will be returned.</param>
    /// <returns>The stat with the given name and type, or null if no stat with the given name and type is found.</returns>
    public Stat? GetStat(string name, StatType type)
    {
        Func<Stat?, bool> filter =
            type == StatType.Any
                ? (Stat? stat) => stat?.Metadata.Name == name
                : (Stat? stat) => stat?.Metadata.Name == name && stat?.Type == type;

        return this.Stats.FirstOrDefault(filter, null);
    }

    /// <summary>
    /// Sets the current value of a resource stat.
    /// </summary>
    /// <param name="name">The name of the resource stat to set.</param>
    /// <param name="value">The value to set the resource stat to.</param>
    /// <returns>The resource stat with the given name, or null if no resource stat with the given name is found.</returns>
    public ResourceStat? SetResourceStat(string name, int value)
    {
        ResourceStat? resourceStat = this.GetStat(name, StatType.Resource) as ResourceStat;
        if (resourceStat == null)
        {
            return null;
        }

        resourceStat.SetCurrentValue(value);
        return resourceStat;
    }

    /// <summary>
    /// Modifies the current value of a resource stat.
    /// </summary>
    /// <param name="name">The name of the resource stat to modify.</param>
    /// <param name="amount">The amount to modify the resource stat by. Positive values increase, negative values decrease.</param>
    /// <returns>The resource stat with the given name, or null if no resource stat with the given name is found.</returns>
    public ResourceStat? ModifyResourceStat(string name, int amount)
    {
        ResourceStat? resourceStat = this.GetStat(name, StatType.Resource) as ResourceStat;
        if (resourceStat == null)
        {
            return null;
        }

        resourceStat.SetCurrentValue(resourceStat.CurrentValue + amount);
        return resourceStat;
    }
}
