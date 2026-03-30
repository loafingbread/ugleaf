namespace GameLogic.Targeting;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Utils;

public interface ITargeterData
{
    public ETargetQuantity QuantityType { get; }
    public List<EFactionRelationship> AllowedTargets { get; }
    public int Count { get; }
}

public record TargeterData : ITargeterData, IDeepCopyable<TargeterData>
{
    public required ETargetQuantity QuantityType { get; init; }
    public required List<EFactionRelationship> AllowedTargets { get; init; }
    public int Count { get; init; }

    [SetsRequiredMembers]
    public TargeterData()
    {
        this.QuantityType = ETargetQuantity.None;
        this.AllowedTargets = new();
        this.Count = 0;
    }

    public TargeterData DeepCopy()
    {
        return new TargeterData()
        {
            QuantityType = this.QuantityType,
            AllowedTargets = [.. this.AllowedTargets],
            Count = this.Count,
        };
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
