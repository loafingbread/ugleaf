namespace GameLogic.Targeting;

public interface ITargeterData
{
    public ETargetQuantity TargetQuantity { get; }
    public List<EFactionRelationship> AllowedTargets { get; }
    public int Count { get; }
}

public record TargeterData : ITargeterData
{
    public required ETargetQuantity TargetQuantity { get; init; } = ETargetQuantity.None;
    public required List<EFactionRelationship> AllowedTargets { get; init; } = new();
    public int Count { get; init; } = 0;
}

public enum ETargetQuantity
{
    None,
    Count,
    All,
}

public enum EFactionRelationship
{
    Any,
    Self,
    Ally,
    Enemy,
    Neutral,
}
