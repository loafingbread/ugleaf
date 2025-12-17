namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public interface IStatBlockSpec
{
    public List<ReferenceSpec> Stats { get; init; }
}

public record StatBlockSpec : IStatBlockSpec
{
    public required List<ReferenceSpec> Stats { get; init; } = new();
}

public interface IStatBlockData
{
    public List<StatData> Stats { get; init; }
}

public record StatBlockData : IStatBlockData
{
    public required List<StatData> Stats { get; init; } = new();
}

public interface IStatSpec
{
    public StatType Type { get; init; }
}

public record StatTemplateSpec : IStatSpec
{
    public required StatMetadata Metadata { get; init; }
    public required IStatConfigData Config { get; init; }
    public required StatType Type { get; init; }
}

public record StatOverrideSpec : ITemplateOverride<StatTemplateSpec>, IStatSpec
{
    public required StatMetadata Metadata { get; init; }
    public required IStatConfigData Config { get; init; }
    public required StatType Type { get; init; }
}

public record StatData : StatTemplateSpec { }

public record StatMetadata
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }
}

public interface IStatConfigData { }

public record ValueStatConfigData : IStatConfigData
{
    public required int BaseValueCap { get; init; }
    public required int CurrentValueCap { get; init; }
    public required StatFormula BaseValueFormula { get; init; }
}

public record ResourceStatConfigData : IStatConfigData
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

public record StatFormulaData
{
    public required StatFormulaType Type { get; init; }
    public required List<StatFormulaData> Operands { get; init; }
    public required int Value { get; init; }
}
