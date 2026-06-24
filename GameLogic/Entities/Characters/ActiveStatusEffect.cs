namespace GameLogic.Entities.Characters;

public record ActiveStatusEffect
{
    public required string VariantName { get; init; }
    public required float Value { get; init; }
    public required float Duration { get; init; }
    public float RemainingTurns { get; set; }
}
