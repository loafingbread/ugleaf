namespace GameLogic.Targeting;

using System.Diagnostics.CodeAnalysis;

public interface ITargeterData
{
    public ETargetQuantity TargetQuantity { get; }
    public List<EFactionRelationship> AllowedTargets { get; }
    public int Count { get; }
}

public record TargeterData : ITargeterData
{
    public required ETargetQuantity TargetQuantity { get; init; }
    public required List<EFactionRelationship> AllowedTargets { get; init; }
    public int Count { get; init; }

    [SetsRequiredMembers]
    public TargeterData()
    {
        this.TargetQuantity = ETargetQuantity.None;
        this.AllowedTargets = new();
        this.Count = 0;
    }
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
