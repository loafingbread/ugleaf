namespace GameLogic.Registry;

using GameLogic.Entities.Stats;
using GameLogic.Usables.Effects;

/// <summary>
/// Central access point for all template and instance registries.
/// Grows one phase at a time as entity types are added.
/// </summary>
public class GameRegistry
{
    public Registry<StatBlock> StatBlocks { get; } = new();
    public Registry<EffectTemplate> Effects { get; } = new();
}
