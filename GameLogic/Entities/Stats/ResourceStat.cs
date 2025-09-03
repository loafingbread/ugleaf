namespace GameLogic.Entities.Stats;

/// <summary>
/// A resource stat is a stat that has a base capacity and a current capacity.
/// The base capacity is the maximum value of the resource with no modifiers.
/// The current capacity is the maximum value of the resource with modifiers.
/// </summary>
public class ResourceStat : Stat
{
    public int BaseCapacity { get; private set; }
    public int CurrentCapacity { get; private set; }

    public ResourceStat(StatRecord record)
        : base(record)
    {
        this.BaseCapacity = this.GetConfig().BaseCapacityFormula.CalculateValue();
        this.CurrentCapacity = this.BaseCapacity;
        this.SetCurrentValue(this.GetConfig().StartingCurrentValue);
    }

    public ResourceStat(ResourceStat resourceStat)
        : base(resourceStat)
    {
        this.BaseCapacity = this.GetConfig().BaseCapacityFormula.CalculateValue();
        this.CurrentCapacity = this.BaseCapacity;
        this.SetCurrentValue(this.GetConfig().StartingCurrentValue);
    }

    public override Stat DeepCopy()
    {
        return new ResourceStat(this);
    }

    public ResourceStatConfigRecord GetConfig()
    {
        return (ResourceStatConfigRecord)this.Config;
    }

    public override bool IsFormulaCalculated()
    {
        return this.GetConfig().BaseCapacityFormula.Type == StatFormulaType.Constant ? false : true;
    }

    public override void OnUpdate()
    {
        if (this.IsFormulaCalculated())
        {
            this.BaseCapacity = this.GetConfig().BaseCapacityFormula.CalculateValue();
        }

        this.CurrentCapacity = this.Modifiers.GetModifiedValueFromBase(
            this.Metadata.Name,
            this.BaseCapacity
        );

        // Set to current value to ensure it is within the current capacity
        this.SetCurrentValue(this.CurrentValue);
    }

    /// <summary>
    /// Sets the base capacity of the stat and updates the current capacity to match.
    /// Base capacity should be the same as the current capacity.
    /// </summary>
    /// <param name="value">
    /// The value to set the base capacity to.
    public void SetBaseCapacity(int value)
    {
        if (this.IsFormulaCalculated())
        {
            System.Console.WriteLine(
                "Warning: Cannot change base capacity of a formula derived stat, only allowed for constant stats."
            );
            return;
        }

        this.BaseCapacity = System.Math.Clamp(value, 0, this.GetConfig().BaseCapacityCap);
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
