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
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<string> Tags { get; set; }
    public required EffectModelData Model { get; set; }

    public EffectStateData DeepCopy()
    {
        return new EffectStateData()
        {
            Name = this.Name,
            Description = this.Description,
            Tags = [.. this.Tags],
            Model = this.Model.DeepCopy(),
        };
    }
}
