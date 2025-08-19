namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;

public interface IStatRecord
{
    // Unique identifier for the stat
    public string Id { get; }

    // Display name for the stat
    public string DisplayName { get; }

    // Description for the stat
    public string Description { get; }

    // Tags for the stat
    public List<string> Tags { get; }

    // Type of the stat
    public StatType Type { get; }

    // Value cap for the stat
    public int ValueCap { get; }

    // Base value formula for the stat
    public StatFormula BaseValueFormula { get; }
}

public enum StatType
{
    Value,
    Resource,
    Add,
    Multiply,
}

public record StatFormulaRecord
{
    public required StatFormulaType Type { get; init; }
    public required List<StatFormulaRecord> Operands { get; init; }
    public required int Value { get; init; }
}

public class StatFormula
{
    public StatFormulaType Type { get; init; }

    // Operands are the operands of the formula if it is type Derived
    public List<StatFormulaRecord> Operands { get; init; }

    // Value is the value of the formula if it is type Constant
    public int Value { get; init; }

    [SetsRequiredMembers]
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

public record StatRecord : IStatRecord
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
    public required StatType Type { get; init; }
    public required int ValueCap { get; init; }
    public required StatFormula BaseValueFormula { get; init; }
    public int CurrentValue { get; init; }
}

public class StatConfig
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
    public required StatType Type { get; init; }
    public required StatFormula BaseValueFormula { get; init; }
    public required int ValueCap { get; init; }

    [SetsRequiredMembers]
    public StatConfig(IStatRecord record)
    {
        this.Id = record.Id;
        this.DisplayName = record.DisplayName;
        this.Description = record.Description;
        this.Tags = record.Tags ?? new List<string>();
        this.Type = record.Type;
        this.BaseValueFormula =
            record.BaseValueFormula
            ?? new StatFormula(StatFormulaType.Constant, new List<StatFormulaRecord>(), 0);
        this.ValueCap = record.ValueCap;
    }
}
