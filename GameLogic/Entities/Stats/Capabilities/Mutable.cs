namespace GameLogic.Entities.Stats.Capabilities;

public record MutableData
{
    public required float Value { get; init; }
}

public interface IMutable
{
    float Value { get; set; }
    float Add(float delta);
    float Set(float value);
}

public class MutableCapability : IMutable
{
    public float Value { get; set; }

    public MutableCapability(float value)
    {
        this.Value = value;
    }

    public float Add(float delta)
    {
        this.Value = this.Value + delta;

        return this.Value;
    }

    public float Set(float value)
    {
        this.Value = value;
        return this.Value;
    }
}
