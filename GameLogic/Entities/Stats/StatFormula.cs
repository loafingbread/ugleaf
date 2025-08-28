namespace GameLogic.Entities.Stats;

using System.Text.Json.Serialization;

public class StatFormula
{
    public StatFormulaType Type { get; init; }

    // Operands are the operands of the formula if it is type Derived
    public List<StatFormulaRecord> Operands { get; init; } = new();

    // Value is the value of the formula if it is type Constant
    public int Value { get; init; }

    // Default constructor for JSON deserialization
    public StatFormula() { }

    public StatFormula(StatFormulaType type, List<StatFormulaRecord> operands, int value)
    {
        this.Type = type;
        this.Operands = operands ?? new List<StatFormulaRecord>();
        this.Value = value == 0 ? 0 : value;
    }

    public int CalculateValue()
    {
        switch (this.Type)
        {
            case StatFormulaType.Constant:
                return this.Value;
            default:
                throw new Exception($"Invalid formula type: {this.Type}");
        }
    }
}

public enum StatFormulaType
{
    Constant,
    Derived,
}
