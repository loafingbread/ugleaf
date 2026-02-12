namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Entities.Stats.Capabilities;
using GameLogic.Registry;

public record StatSpec
{
    public required StatMetadata Metadata { get; init; }
    public required ValueModelData ValueModel { get; init; }
    public required StatCapabilities Capabilities { get; init; }
}

// TODO: StatSpec seems unneeded? Just use StatData?

public record StatPatch
{
    public StatMetadata? Metadata { get; init; }
    public ValueModelData? ValueModel { get; init; }
    public StatCapabilities? Capabilities { get; init; }

    public StatData ApplyTo(StatData baseData)
    {
        return new StatData(
            baseData.ReferenceId,
            baseData.ValueModel,
            this.Metadata ?? baseData.Metadata,
            this.Capabilities ?? baseData.Capabilities
        );
    }
}
