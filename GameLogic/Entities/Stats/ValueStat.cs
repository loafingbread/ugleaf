namespace GameLogic.Entities.Stats;

public class ValueStat
{
    public ValueStatMetadata Metadata { get; init; }

    public int BaseValue { get; private set; }
    public int CurrentValue { get; private set; }

    private StatModifiers _modifiers { get; init; }

    public ValueStat(ValueStatMetadata metadata, StatModifiers modifiers)
    {
        this.Metadata = metadata;

        this.BaseValue = metadata.BaseValueFormula.CalculateValue();
        this.CurrentValue = this.BaseValue;

        this._modifiers = modifiers;
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
    public bool IsFormulaCalculated()
    {
        return this.Metadata.BaseValueFormula.Type == StatFormulaType.Constant ? false : true;
    }

    /// <summary>
    /// Changes the base value of the stat by the specified amount.
    /// Only allowed for stats with a constant formula type (<see cref="StatFormulaType.Constant"/>).
    /// </summary>
    /// <param name="amount">
    /// The amount to adjust the base value by. Positive values increase, negative values decrease.
    /// </param>
    public void SetBaseValue(int amount)
    {
        if (this.IsFormulaCalculated())
        {
            System.Console.WriteLine(
                "Warning: Cannot change base value of a formula derived stat, only allowed for constant stats."
            );
            return;
        }

        this.BaseValue = System.Math.Clamp(this.BaseValue + amount, 0, this.Metadata.BaseValueCap);
    }

    /// <summary>
    /// Refreshes stat value to reflect the current base value and current modifiers.
    ///
    /// Recalculates the base value if the stat is derived from a formula otherwise
    /// it uses the current base value.
    ///
    /// Should be called before reading the stat to get latest value.
    /// </summary>
    public void Refresh()
    {
        if (this.IsFormulaCalculated())
        {
            this.BaseValue = this.Metadata.BaseValueFormula.CalculateValue();
        }

        this.CurrentValue = this._modifiers.GetModifiedValueFromBase(
            this.Metadata.Id,
            this.BaseValue
        );
    }
}

public class ValueStatMetadata : StatMetadata
{
    public required int BaseValueCap { get; init; }
    public required StatFormula BaseValueFormula { get; init; }
    public required int CurrentValueCap { get; init; }
}
