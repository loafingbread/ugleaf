using GameLogic.Entities.Stats.Stat;

public record ConstantData
{
    public required float Value { get; init; }

    public ConstantData DeepCopy()
    {
        return new ConstantData() { Value = this.Value };
    }

    public bool IsValid()
    {
        if (this.Value is float.NaN)
        {
            return false;
        }

        return true;
    }
}
