namespace GameLogic.Entities.Stats.StatBlock;

using GameLogic.Entities.Stats.Modifiers;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;
using GameLogic.Utils;

public class StatBlock : IDeepCopyable<StatBlock>
{
    public List<Stat> Stats { get; private set; } = new();
    public StatModifiers Modifiers { get; private set; } = new();

    public StatBlock(List<Stat> stats)
    {
        this.Stats = [.. stats];
    }

    public StatBlock(StatBlock statBlock)
    {
        this.Stats = statBlock.Stats.DeepCopyList();
        this.Modifiers = statBlock.Modifiers.DeepCopy();
    }

    public StatBlock DeepCopy()
    {
        return new StatBlock(this);
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
    public Stat? GetStat(ReferenceId referenceId)
    {
        Func<Stat?, bool> filter = (Stat? stat) => stat?.ReferenceId == referenceId;

        return this.Stats.FirstOrDefault(filter, null);
    }
}
