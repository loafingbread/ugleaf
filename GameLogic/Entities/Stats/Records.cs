namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

public interface IStatBlockRecord
{
    public List<StatRecord> Stats { get; init; }
}

public record StatBlockRecord : IStatBlockRecord
{
    public required List<StatRecord> Stats { get; init; } = new();
}

public record StatRecord
{
    public required StatMetadataRecord Metadata { get; init; }
    public required IStatConfigRecord Config { get; init; }
    public required StatType Type { get; init; }
}

public record StatMetadataRecord
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
}

public interface IStatConfigRecord { }

public record ValueStatConfigRecord : IStatConfigRecord
{
    public required int BaseValueCap { get; init; }
    public required int CurrentValueCap { get; init; }
    public required StatFormula BaseValueFormula { get; init; }
}

public record ResourceStatConfigRecord : IStatConfigRecord
{
    public required int BaseCapacityCap { get; init; }
    public required int CurrentCapacityCap { get; init; }
    public required StatFormula BaseCapacityFormula { get; init; }
    public required int StartingCurrentValue { get; init; }
}

public enum StatType
{
    Value,
    Resource,
    Any,
}

public record StatFormulaRecord
{
    public required StatFormulaType Type { get; init; }
    public required List<StatFormulaRecord> Operands { get; init; }
    public required int Value { get; init; }
}
