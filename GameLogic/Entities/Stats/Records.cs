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

public record StatSpec
{
    public required StatMetadata Metadata { get; init; }
    public required StatState State { get; init; }
    public required StatCapabilities Capabilities { get; init; }
}

public record StatCapabilities
{
    public BoundsData? Bounds { get; init; } = null;
    public MaxData? Max { get; init; } = null;
    public FormulaData? ImmutableValue { get; init; } = null;

    public bool HasBounds => this.Bounds is not null;
    public bool HasMax => this.Max is not null;
    public bool HasImmutableValue => this.ImmutableValue is not null;
    public bool HasMutableValue => this.ImmutableValue is null;
}

public record BoundsData
{
    public required float LowerBound { get; init; }
    public required float UpperBound { get; init; }
}

public record MaxData
{
    public required ReferenceId MaxStatId { get; init; }
}

public record FormulaData
{
    public required StatFormulaType Type { get; init; }
    public required List<StatFormulaData> Operands { get; init; }
    public required int Value { get; init; }
}

public record StatOverrideSpec : ITemplateOverride<StatSpec>
{
    public required StatMetadata Metadata { get; init; }
    public required IStatConfigData Config { get; init; }
}

public record StatState
{
    public required float Value { get; init; }
}

public record StatData : StatSpec
{
    public required ReferenceId ReferenceId { get; init; }
}

public record StatMetadata
{
    public required StatType Type { get; init; }
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
