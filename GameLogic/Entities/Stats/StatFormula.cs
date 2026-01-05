namespace GameLogic.Entities.Stats;

using System.Text.Json.Serialization;

public class StatFormula
{
    public required StatFormulaData Data { get; init; }

    // Default constructor for JSON deserialization
    public StatFormula() { }

    public StatFormula(StatFormulaData data)
    {
        this.Data = data;
    }

    public int CalculateValue()
    {
        switch (this.Data.Type)
        {
            case StatFormulaType.Constant:
                return this.Data.Value;
            default:
                throw new Exception($"Invalid formula type: {this.Data.Type}");
        }
    }
}

public enum StatFormulaType
{
    Constant,
    Derived,
}
