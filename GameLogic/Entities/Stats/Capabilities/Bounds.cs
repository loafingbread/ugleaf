namespace GameLogic.Entities.Stats.Capabilities;

public record BoundsData
{
    public required float LowerBound { get; init; }
    public required float UpperBound { get; init; }

    public BoundsData DeepCopy()
    {
        return new BoundsData() { LowerBound = this.LowerBound, UpperBound = this.UpperBound };
    }
}

public interface IBounds
{
    float LowerBound { get; set; }
    float UpperBound { get; set; }
    float ApplyBounds(float value);
}

public class BoundsCapability : IBounds
{
    public float LowerBound { get; set; }
    public float UpperBound { get; set; }

    public BoundsCapability(float lowerBound, float upperBound)
    {
        this.LowerBound = lowerBound;
        this.UpperBound = upperBound;
    }

    public float ApplyBounds(float value)
    {
        return System.Math.Clamp(value, this.LowerBound, this.UpperBound);
    }
}
