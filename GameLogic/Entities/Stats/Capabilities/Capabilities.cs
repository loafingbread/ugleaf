namespace GameLogic.Entities.Stats.Capabilities;

public record StatCapabilities
{
    public BoundsData? Bounds { get; init; } = null;
    public MaxData? Max { get; init; } = null;
    public ImmutableSpec? ImmutableValue { get; init; } = null;
    public MutableData? MutableValue { get; init; } = null;

    public bool HasBounds => this.Bounds is not null;
    public bool HasMax => this.Max is not null;
    public bool HasImmutableValue => this.ImmutableValue is not null;
    public bool HasMutableValue => this.MutableValue is not null;
}
