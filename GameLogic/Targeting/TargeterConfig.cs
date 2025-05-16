namespace GameLogic.Targeting;

public record TargeterRecord
{
    public required ETargetQuantity TargetQuantity { get; init; }
    public required List<EFactionRelationship> AllowedTargets { get; init; }
    public int Count { get; init; }
}

public class TargeterConfig
{
    public ETargetQuantity TargetQuantity { get; private set; }
    public List<EFactionRelationship> AllowedTargets { get; private set; }
    public int Count { get; private set; }

    public TargeterConfig(TargeterRecord record)
    {
        this.TargetQuantity = record.TargetQuantity;
        this.AllowedTargets = [.. record.AllowedTargets];
        this.Count = record.Count;
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
    Self,
    Ally,
    Enemy,
    Neutral,
}
