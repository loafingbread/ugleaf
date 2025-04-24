namespace GameLogic.Targeting;

public record TargeterConfig
{
    public required ETargetQuantity TargetQuantity { get; init; }
    public required List<EFactionRelationship> AllowedTargets { get; init; }
    public int Count { get; init; }
}

public enum ETargetQuantity
{
    None,
    Count,
    All,
}

public enum EFactionRelationship
{
    Self,
    Ally,
    Enemy,
    Neutral,
}
