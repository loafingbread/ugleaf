namespace GameLogic.Registry;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

public class GameRegistry
{
    public Registry<StatBlock> StatBlocks { get; } = new();
    public Registry<EffectTemplate> Effects { get; } = new();
    public Registry<UsableTemplate> Usables { get; } = new();
    public Registry<SkillTemplate> Skills { get; } = new();
}
