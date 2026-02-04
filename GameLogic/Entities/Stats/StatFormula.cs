namespace GameLogic.Entities.Stats;

using System.Text.Json.Serialization;

public class StatFormula
{
    public required FormulaData Data { get; init; }

    // TODO: Remove default constructor for JSON deserialization if not needed
    // public StatFormula() { }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatFormula(FormulaData data)
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
