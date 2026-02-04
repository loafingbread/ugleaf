namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;
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

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatSpec(StatMetadata metadata, StatState state, StatCapabilities capabilities)
    {
        this.Metadata = metadata;
        this.State = state;
        this.Capabilities = capabilities;
    }
}

public record StatCapabilities
{
    public BoundsData? Bounds { get; init; } = null;
    public MaxData? Max { get; init; } = null;
    public ImmutableData? ImmutableValue { get; init; } = null;
    public MutableData? MutableValue { get; init; } = null;

    public bool HasBounds => this.Bounds is not null;
    public bool HasMax => this.Max is not null;
    public bool HasImmutableValue => this.ImmutableValue is not null;
    public bool HasMutableValue => this.MutableValue is not null;
}

public record BoundsData
{
    public required float LowerBound { get; init; }
    public required float UpperBound { get; init; }
}

public record MaxData
{
    /// <summary>
    /// The reference id of the stat to use as the max value.
    /// </summary>
    public required ReferenceId MaxStatId { get; init; }

    /// <summary>
    /// Whether to use the base value or the current value of the max stat.
    /// </summary>
    public required bool ByBaseValue { get; init; }
}

public record MutableData
{
    public required float Value { get; init; }
}

public record ImmutableData : FormulaData { }

public record FormulaData
{
    public required StatFormulaType Type { get; init; }
    public required List<FormulaData> Operands { get; init; }
    public required int Value { get; init; }
}

public record StatOverrideSpec : ITemplateOverride<StatSpec>
{
    public StatMetadata? Metadata { get; init; }
    public StatState? State { get; init; }
    public StatCapabilities? Capabilities { get; init; }
}

public record StatState
{
    public required float Value { get; init; } = 0f;

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatState() { }
}

public record StatData
{
    public required ReferenceId ReferenceId { get; init; }
    public StatMetadata Metadata { get; set; } = new();
    public StatState State { get; set; } = new();
    public StatCapabilities Capabilities { get; set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatData(ReferenceId referenceId)
    {
        this.ReferenceId = referenceId;
    }

    public void Resolve(ReferenceSpec spec, Func<ReferenceId?, StatData> getStatData)
    {
        switch (spec.Metadata.Kind)
        {
            case EReferenceKind.Inline:
            {
                this.ResolveInline(spec);
                return;
            }
            case EReferenceKind.Override:
            {
                this.ResolveOverride(spec, getStatData);
                return;
            }
            case EReferenceKind.Instance:
            {
                this.ResolveInstance(spec);
                return;
            }
            default:
            {
                // Does not support reference kind
                throw new InvalidOperationException("Invalid stat spec kind");
            }
        }
    }

    private void ResolveInline(ReferenceSpec spec)
    {
        var inlineSpec = spec as InlineSpec<StatSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a stat template");
        }

        this.Metadata = inlineSpec.Template.Metadata;
        this.State = inlineSpec.Template.State;
        this.Capabilities = inlineSpec.Template.Capabilities;
    }

    private void ResolveOverride(ReferenceSpec spec, Func<ReferenceId?, StatData> getStatData)
    {
        var overrideSpec = spec as OverrideSpec<StatSpec, StatOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a stat template");
        }

        StatData statData = getStatData(overrideSpec.Metadata.DependencyId);

        this.Metadata = overrideSpec.Override.Metadata ?? statData.Metadata;
        this.State = overrideSpec.Override.State ?? statData.State;
        this.Capabilities = overrideSpec.Override.Capabilities ?? statData.Capabilities;
    }

    private void ResolveInstance(ReferenceSpec spec)
    {
        var instanceSpec = spec as InstanceSpec<StatSpec, StatData>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a stat template");
        }

        this.Metadata = instanceSpec.Instance.Metadata;
        this.State = instanceSpec.Instance.State;
        this.Capabilities = instanceSpec.Instance.Capabilities;
    }
}

public record StatMetadata
{
    public required StatType Type { get; init; } = StatType.Any;
    public required string Name { get; init; } = "";
    public required string DisplayName { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatMetadata() { }
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
