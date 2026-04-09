namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

/// <summary>
/// A reference to an effect instance. Only defines a data class instead of a
/// spec class because specs are for definitions that need references to be
/// resolved. Effect should not have any references to other types, as it
/// is a leaf level type.
/// </summary>
public record EffectInstanceRef : InstanceRef<EffectStateData> { }

public record EffectStateData
{
    public required string Type { get; set; }
    public required string Subtype { get; set; }
    public required string Variant { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<string> Tags { get; set; }
    public required EffectConfigData Config { get; set; }

    public EffectStateData DeepCopy()
    {
        return new EffectStateData()
        {
            Type = this.Type,
            Subtype = this.Subtype,
            Variant = this.Variant,
            Name = this.Name,
            Description = this.Description,
            Tags = [.. this.Tags],
            Config = this.Config.DeepCopy(),
        };
    }
}
