namespace GameLogic.Actions.Effects;

public record EffectStateData
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    /// <summary>"Attack" | "Heal" | "Status"</summary>
    public required string Type { get; init; }

    /// <summary>"SingleHit" | "Burn" | "Poison" etc.</summary>
    public required string Subtype { get; init; }

    public required EffectConfigData Config { get; init; }
}
