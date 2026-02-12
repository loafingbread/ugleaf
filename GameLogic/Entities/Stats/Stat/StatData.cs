namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Entities.Stats.Capabilities;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Registry;

public record StatTemplate
{
    public required ReferenceId ReferenceId { get; init; }
    public required ValueModelData ValueModel { get; init; }
    public StatMetadata Metadata { get; set; } = new();
    public StatCapabilities Capabilities { get; set; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatTemplate(
        ReferenceId referenceId,
        ValueModelData valueModel,
        StatMetadata metadata,
        StatCapabilities capabilities
    )
    {
        this.ReferenceId = referenceId;
        this.ValueModel = valueModel;
        this.Metadata = metadata.DeepCopy();
        this.Capabilities = capabilities.DeepCopy();
    }

    public StatTemplate DeepCopy()
    {
        return new StatTemplate(
            this.ReferenceId,
            this.ValueModel,
            this.Metadata,
            this.Capabilities
        );
    }
}

public record StatMetadata
{
    public required string Name { get; init; } = "";
    public required string DisplayName { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatMetadata() { }

    public StatMetadata DeepCopy()
    {
        return new StatMetadata()
        {
            Name = this.Name,
            DisplayName = this.DisplayName,
            Description = this.Description,
            Tags = [.. this.Tags],
        };
    }
}
