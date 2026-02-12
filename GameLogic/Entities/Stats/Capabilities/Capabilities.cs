namespace GameLogic.Entities.Stats.Capabilities;

public record StatCapabilities
{
    public BoundsData? Bounds { get; init; } = null;
    public MaxData? Max { get; init; } = null;

    public bool HasBounds => this.Bounds is not null;
    public bool HasMax => this.Max is not null;

    public StatCapabilities DeepCopy()
    {
        return new StatCapabilities()
        {
            Bounds = this.Bounds?.DeepCopy(),
            Max = this.Max?.DeepCopy(),
        };
    }
}
