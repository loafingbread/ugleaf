using GameLogic.Entities;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Usables.Effects;
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

    public static CommandResult DisplayCharacters(
        List<Character> characters,
        ConsoleUI ui,
        int indentLevel = 0
    )
    {
        ui.PrintSubsection($"Characters ({characters.Count})", indentLevel);

        for (int i = 0; i < characters.Count; i++)
        {
            bool isLast = i == characters.Count - 1;
            DisplayCharacter(characters[i], ui, indentLevel + 1, isLast);
        }

        return CommandResult.Ok();
    }

    public static CommandResult DisplayCharacterById(
        string characterId,
        GameState gameState,
        ConsoleUI ui,
        int indentLevel = 0
    )
    {
        Character? character = gameState.PlayerState.Characters.FirstOrDefault(c =>
            c.GetConfig().Id == characterId
        );
        if (character == null)
            return CommandResult.Error($"Character not found: {characterId}");

        DisplayCharacter(character, ui, indentLevel);
        return CommandResult.Ok();
    }

    public static void DisplayCharacter(
        Character character,
        ConsoleUI ui,
        int indentLevel = 0,
        bool isLast = false
    )
    {
        ui.PrintSubsection(
            $"{character.GetConfig().Name} ({character.GetConfig().Id})",
            indentLevel,
            isLast
        );

        DisplayStats(character, ui, indentLevel + 1);
        DisplaySkills(character, ui, indentLevel + 1);
    }

    public static void DisplayStats(Character character, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintSubsection(
            $"{character.GetConfig().Name} Stats ({character.Stats.Stats.Count})",
            indentLevel
        );

        Stat? strengthStat = character.Stats.GetStat("value_stat_strength", StatType.Value);
        ui.PrintIndentedInfo(
            $"{strengthStat?.Metadata.DisplayName}: {strengthStat?.CurrentValue}",
            indentLevel + 1
        );

        Stat? constitutionStat = character.Stats.GetStat("value_stat_constitution", StatType.Value);
        ui.PrintIndentedInfo(
            $"{constitutionStat?.Metadata.DisplayName}: {constitutionStat?.CurrentValue}",
            indentLevel + 1
        );

        Stat? intelligenceStat = character.Stats.GetStat("value_stat_intelligence", StatType.Value);
        ui.PrintIndentedInfo(
            $"{intelligenceStat?.Metadata.DisplayName}: {intelligenceStat?.CurrentValue}",
            indentLevel + 1
        );

        Stat? agilityStat = character.Stats.GetStat("value_stat_agility", StatType.Value);
        ui.PrintIndentedInfo(
            $"{agilityStat?.Metadata.DisplayName}: {agilityStat?.CurrentValue}",
            indentLevel + 1
        );

        ResourceStat? healthStat =
            character.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
        ui.PrintIndentedInfo(
            $"{healthStat?.Metadata.DisplayName}: {healthStat?.CurrentValue} / {healthStat?.CurrentCapacity}",
            indentLevel + 1
        );

        ResourceStat? manaStat =
            character.Stats.GetStat("resource_stat_mana", StatType.Resource) as ResourceStat;
        ui.PrintIndentedInfo(
            $"{manaStat?.Metadata.DisplayName}: {manaStat?.CurrentValue} / {manaStat?.CurrentCapacity}",
            indentLevel + 1
        );
    }

    public static void DisplaySkills(Character character, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintSubsection(
            $"{character.GetConfig().Name} Skills ({character.GetConfig().Skills.Count})",
            indentLevel
        );

        for (int i = 0; i < character.GetConfig().Skills.Count; i++)
        {
            bool isLast = i == character.GetConfig().Skills.Count - 1;
            DisplaySkill(character.GetConfig().Skills[i], ui, indentLevel + 1, isLast);
        }
    }

    public static void DisplaySkill(
        Skill skill,
        ConsoleUI ui,
        int indentLevel = 0,
        bool isLast = false
    )
    {
        ui.PrintSubsection(
            $"{skill.GetConfig().Name} ({skill.GetConfig().Id})",
            indentLevel,
            isLast
        );

        DisplayTargeter(skill.Targeter?.GetConfig(), ui, indentLevel + 1);
        DisplayUsables(skill.Usables, ui, indentLevel + 1);
    }

    public static void DisplayTargeter(TargeterConfig? targeter, ConsoleUI ui, int indentLevel = 0)
    {
        if (targeter == null)
            return;

        ui.PrintSubsection("Targeter", indentLevel);

        ui.PrintIndentedInfo($"Target Quantity Type: {targeter.TargetQuantity}", indentLevel + 1);
        ui.PrintIndentedInfo(
            $"Allowed Target Types: {string.Join(", ", targeter.AllowedTargets)}",
            indentLevel + 1
        );
        ui.PrintIndentedInfo($"Target Count: {targeter.Count}", indentLevel + 1);
    }

    public static void DisplayUsables(List<IUsable> usables, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintSubsection($"Usables ({usables.Count})", indentLevel);

        for (int i = 0; i < usables.Count; i++)
        {
            bool isLast = i == usables.Count - 1;
            DisplayUsable(usables[i], ui, indentLevel + 1, isLast);
        }
    }

    public static void DisplayUsable(
        IUsable usable,
        ConsoleUI ui,
        int indentLevel = 0,
        bool isLast = false
    )
    {
        ui.PrintSubsection($"{usable.GetConfig().Id}", indentLevel, isLast);

        TargeterConfig usableTargeter = usable.GetConfig().Targeter;
        DisplayTargeter(usableTargeter, ui, indentLevel + 1);
        DisplayEffects(usable.GetConfig().Effects, ui, indentLevel + 1);
    }

    public static void DisplayEffects(List<IEffect> effects, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintSubsection($"Effects ({effects.Count})", indentLevel);

        for (int i = 0; i < effects.Count; i++)
        {
            bool isLast = i == effects.Count - 1;
            DisplayEffect(effects[i], ui, indentLevel + 1, isLast);
        }
    }

    public static void DisplayEffect(
        IEffect effect,
        ConsoleUI ui,
        int indentLevel = 0,
        bool isLast = false
    )
    {
        ui.PrintSubsection($"{effect.GetConfig().Id}", indentLevel, isLast);

        ui.PrintIndentedInfo($"Type: {effect.GetConfig().Type}", indentLevel + 1);
        ui.PrintIndentedInfo($"Subtype: {effect.GetConfig().Subtype}", indentLevel + 1);
        ui.PrintIndentedInfo($"Variant: {effect.GetConfig().Variant}", indentLevel + 1, isLast);
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
