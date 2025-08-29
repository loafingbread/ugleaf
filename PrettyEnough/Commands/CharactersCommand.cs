using GameLogic.Entities;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Stats;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class CharactersCommand : ICommand
{
    public string Name => "characters";
    public string Description => "Display all current characters in a pretty format";
    public string Usage => "characters";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        if (gameState?.PlayerState?.Characters == null)
            return CommandResult.Error("No player characters available");

        if (args.Length == 0)
        {
            return await Task.FromResult(DisplayCharacters(gameState.PlayerState.Characters, ui));
        }
        else
        {
            string characterId = args[0];
            return await Task.FromResult(DisplayCharacterById(characterId, gameState, ui));
        }
    }

    public static CommandResult DisplayCharacters(List<Character> characters, ConsoleUI ui)
    {
        ui.PrintSection("📊 Current Characters");

        foreach (var character in characters)
        {
            DisplayCharacter(character, ui);
        }

        return CommandResult.Ok();
    }

    public static CommandResult DisplayCharacterById(
        string characterId,
        GameState gameState,
        ConsoleUI ui
    )
    {
        Character? character = gameState.PlayerState.Characters.FirstOrDefault(c =>
            c.GetConfig().Id == characterId
        );
        if (character == null)
            return CommandResult.Error($"Character not found: {characterId}");

        DisplayCharacter(character, ui);
        return CommandResult.Ok();
    }

    public static void DisplayCharacter(Character character, ConsoleUI ui)
    {
        ui.PrintSection($"📊 {character.GetConfig().Name} ({character.GetConfig().Id})");

        Stat strengthStat = character.Stats.GetStat("value_stat_strength", StatType.Value);
        ui.PrintInfo($"   {strengthStat?.Metadata.DisplayName}: {strengthStat?.CurrentValue}");

        Stat constitutionStat = character.Stats.GetStat("value_stat_constitution", StatType.Value);
        ui.PrintInfo(
            $"   {constitutionStat?.Metadata.DisplayName}: {constitutionStat?.CurrentValue}"
        );

        Stat intelligenceStat = character.Stats.GetStat("value_stat_intelligence", StatType.Value);
        ui.PrintInfo(
            $"   {intelligenceStat?.Metadata.DisplayName}: {intelligenceStat?.CurrentValue}"
        );

        Stat agilityStat = character.Stats.GetStat("value_stat_agility", StatType.Value);
        ui.PrintInfo($"   {agilityStat?.Metadata.DisplayName}: {agilityStat?.CurrentValue}");

        ResourceStat healthStat =
            character.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
        ui.PrintInfo(
            $"   {healthStat?.Metadata.DisplayName}: {healthStat?.CurrentValue} / {healthStat?.CurrentCapacity}"
        );

        ResourceStat manaStat =
            character.Stats.GetStat("resource_stat_mana", StatType.Resource) as ResourceStat;
        ui.PrintInfo(
            $"   {manaStat?.Metadata.DisplayName}: {manaStat?.CurrentValue} / {manaStat?.CurrentCapacity}"
        );

        ui.PrintInfo("");
    }

    // private void DisplayCharacterStats(Character character, ConsoleUI ui)
    // {
    //     ui.PrintInfo($"{character.Name} ({character.Id})");
    // }

    // private void DisplayCharacterResources(Character character, ConsoleUI ui)
    // {
    //     ui.PrintInfo($"{character.Name} ({character.Id})");
    // }

    // private void DisplayCharacterSkills(Character character, ConsoleUI ui)
    // {
    //     ui.PrintInfo($"{character.Name} ({character.Id})");
    // }

    // private void DisplayCharacterInventory(Character character, ConsoleUI ui)
    // {
    //     ui.PrintInfo($"{character.Name} ({character.Id})");
    // }

    // private void DisplayCharacterEquipment(Character character, ConsoleUI ui)
    // {
    //     ui.PrintInfo($"{character.Name} ({character.Id})");
    // }
}

// public class StatsCommand : ICommand
// {
//     public string Name => "stats";
//     public string Description => "Display all current stats in a pretty format";
//     public string Usage => "stats [stat_name]";

//     public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
//     {
//         if (gameState?.StatBlock == null)
//             return CommandResult.Error("No game state available");

//         if (args.Length > 0)
//         {
//             // Show specific stat
//             var statName = args[0];
//             var stat = gameState.StatBlock.GetStat(statName, StatType.Any);

//             if (stat == null)
//                 return CommandResult.Error($"Stat not found: {statName}");

//             DisplayStat(stat, ui);
//             return CommandResult.Ok();
//         }

//         // Show all stats
//         ui.PrintSection("📊 Current Stats");

//         if (gameState.StatBlock.Stats.Count == 0)
//         {
//             ui.PrintWarning("No stats available");
//             return CommandResult.Ok();
//         }

//         foreach (var stat in gameState.StatBlock.Stats)
//         {
//             DisplayStat(stat, ui);
//         }

//         return await Task.FromResult(CommandResult.Ok());
//     }

//     private void DisplayStat(Stat stat, ConsoleUI ui)
//     {
//         var statType = stat.Type switch
//         {
//             StatType.Value => "📈",
//             StatType.Resource => "💧",
//             _ => "❓",
//         };

//         ui.PrintInfo($"{statType} {stat.Metadata.DisplayName} ({stat.Metadata.Name})");
//         ui.PrintInfo($"   Current: {stat.CurrentValue}");
//         ui.PrintInfo($"   Base: {stat.BaseValue}");
//         ui.PrintInfo($"   Type: {stat.Type}");
//         ui.PrintInfo($"   Description: {stat.Metadata.Description}");
//         ui.PrintInfo($"   Tags: {string.Join(", ", stat.Metadata.Tags)}");

//         // Show additional info for resource stats
//         if (stat is ResourceStat resourceStat)
//         {
//             ui.PrintInfo(
//                 $"   Capacity: {resourceStat.CurrentValue}/{resourceStat.GetConfig().CurrentCapacityCap}"
//             );
//         }

//         ui.PrintInfo("");
//     }
// }
