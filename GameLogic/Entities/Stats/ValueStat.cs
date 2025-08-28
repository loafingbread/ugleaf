namespace GameLogic.Entities.Stats;

/// <summary>
/// A value stat is a stat that has a base value and a current value.
/// The base value is the value of the stat with no modifiers.
/// The current value is the value of the stat with modifiers.
/// </summary>
public class ValueStat : Stat
{
    public ValueStat(StatRecord record)
        : base(record)
    {
        this.BaseValue = this.GetConfig().BaseValueFormula.CalculateValue();
        this.CurrentValue = this.BaseValue;
    }

    public ValueStatConfigRecord GetConfig()
    {
        return (ValueStatConfigRecord)this.Config;
    }

    public override bool IsFormulaCalculated()
    {
        return this.GetConfig().BaseValueFormula.Type == StatFormulaType.Constant ? false : true;
    }

    public override void OnUpdate()
    {
        if (this.IsFormulaCalculated())
        {
            this.BaseValue = this.GetConfig().BaseValueFormula.CalculateValue();
        }

        this.CurrentValue = this.Modifiers.GetModifiedValueFromBase(
            this.Metadata.Name,
            this.BaseValue
        );
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

        this.BaseValue = System.Math.Clamp(
            this.BaseValue + amount,
            0,
            this.GetConfig().BaseValueCap
        );
    }
}
