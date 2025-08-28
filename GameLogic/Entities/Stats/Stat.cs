namespace GameLogic.Entities.Stats;

/// <summary>
/// A stat is a value that can be modified by modifiers.
/// </summary>
public abstract class Stat
{
    public StatMetadataRecord Metadata { get; init; }
    public IStatConfigRecord Config { get; init; }
    public StatType Type { get; init; }

    public StatModifiers Modifiers { get; private set; } = new();
    public int BaseValue { get; protected set; }
    public int CurrentValue { get; protected set; }

    public Stat(StatRecord record)
    {
        this.Metadata = record.Metadata;
        this.Config = record.Config;
        this.Type = record.Type;
        this.Modifiers = new StatModifiers();
    }

    /// <summary>
    /// Checks if the stat is derived from a formula or a constant.
    ///
    /// If the stat is derived from a formula, the base value should be calculated using the formula
    /// every time the stat is refreshed.
    ///
    /// If the stat is a constant, the base value should be set using the constant formula and
    /// updated with ChangeBaseValue method. Formula should never be used except during initialization.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the stat is derived from a formula, <c>false</c> if it is a constant.
    /// </returns>
    public abstract bool IsFormulaCalculated();

    /// <summary>
    /// Updates the stat value to reflect the current base value and current modifiers.
    ///
    /// Recalculates the base value if the stat is derived from a formula otherwise
    /// it uses the current base value.
    ///
    /// Should be called when the stat is updated.
    /// </summary>
    public abstract void OnUpdate();
}
