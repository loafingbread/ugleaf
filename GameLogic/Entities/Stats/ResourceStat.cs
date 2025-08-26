namespace GameLogic.Entities.Stats;

class ResourceStat
{
    public ResourceStatMetadata Metadata { get; init; }
    public int BaseCapacity { get; private set; }
    public int CurrentCapacity { get; private set; }
    public int BaseValue { get; private set; }
    public int CurrentValue { get; private set; }

    private StatModifiers _modifiers { get; init; }

    public ResourceStat(ResourceStatMetadata metadata, StatModifiers modifiers)
    {
        this.Metadata = metadata;

        this.BaseCapacity = metadata.BaseCapacityFormula.CalculateValue();
        this.CurrentCapacity = this.BaseCapacity;
        this.CurrentValue = this.SetCurrentValue(metadata.StartingCurrentValue);

        this._modifiers = modifiers;
    }

    void IsFormulaCalculated()
    {
        return this.Metadata.BaseCapacityFormula.Type == StatFormulaType.Constant ? false : true;
    }

    public void Refresh()
    {
        if (this.IsFormulaCalculated())
        {
            this.BaseCapacity = this.Metadata.BaseCapacityFormula.CalculateValue();
        }

        this.CurrentCapacity = this._modifiers.GetModifiedValueFromBase(
            this.Metadata.Id,
            this.BaseCapacity
        );

        // Set to current value to ensure it is within the current capacity
        this.SetCurrentValue(this.CurrentValue);
    }

    public void SetBaseCapacity(int value)
    {
        if (this.IsFormulaCalculated())
        {
            System.Console.WriteLine(
                "Warning: Cannot change base capacity of a formula derived stat, only allowed for constant stats."
            );
            return;
        }

        this.BaseCapacity = System.Math.Clamp(
            this.BaseCapacity + amount,
            0,
            this.Metadata.BaseCapacityCap
        );
    }

    /// <summary>
    /// Sets the current value of the stat and updates the base value to match.
    /// Base value should be the same as the current value. Used for maintaining
    /// consistent interface for stats.
    /// </summary>
    /// <param name="value">
    /// The value to set the current value to.
    /// </param>
    public void SetCurrentValue(int value)
    {
        this.CurrentValue = System.Math.Clamp(value, 0, this.CurrentCapacity);
        this.BaseValue = this.CurrentValue;
    }
}

class ResourceStatMetadata : StatMetadata
{
    public required int BaseCapacityCap { get; init; }
    public required int CurrentCapacityCap { get; init; }
    public required StatFormula BaseCapacityFormula { get; init; }
    public required int StartingCurrentValue { get; init; }
}
