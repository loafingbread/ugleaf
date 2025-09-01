using GameLogic.Entities;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Usables.Effects;
using PrettyEnough.UI;
using PrettyEnough.Utils;

namespace PrettyEnough.Commands;

public class CharactersCommand : BaseCommand
{
    public override string Name => "characters";
    public override string Description => "Display all player characters in a pretty format";
    public override string Usage => "characters";

    public override string DetailedHelp =>
        "The characters command displays information about all player characters in the game. "
        + "It can be used to list all characters, show a summary of all characters, or display a specific character. "
        + "The command also supports various flags to filter the output, such as showing only a specific character or displaying detailed information.";

    public override Dictionary<string, string> Subcommands =>
        new()
        {
            { "list", "List all characters" },
            { "summary", "Show a summary of all characters" },
        };

    public override Dictionary<string, string> Flags =>
        new()
        {
            { "--help, -h", "Show detailed help for this command" },
            { "--character, -c", "Show a specific character" },
        };

    protected override async Task<CommandResult> ExecuteCommand(
        ArgParser argParser,
        GameState? gameState,
        ConsoleUI ui
    )
    {
        if (gameState?.PlayerState?.Characters == null)
            return CommandResult.Error("No player characters available");

        if (argParser.PositionalArgs.Count == 0)
        {
            return CharactersCommand.DefaultSubcommand(gameState, ui, argParser);
        }

        string subcommand = argParser.PositionalArgs[0].ToLower();
        argParser.PositionalArgs.RemoveAt(0);

        return subcommand switch
        {
            "list" => CharactersCommand.ListSubcommand(gameState, ui, 0),
            "summary" => CharactersCommand.SummarySubcommand(gameState, ui, 0),
            _ => CommandResult.Error(
                $"Unknown subcommand: {subcommand}. Use 'characters --help' for available options."
            ),
        };
    }

    private static CommandResult DefaultSubcommand(
        GameState gameState,
        ConsoleUI ui,
        ArgParser argParser
    )
    {
        if (argParser.HasFlag("character", "c"))
        {
            string characterId = argParser.GetFlag("character", "c");
            return CharactersCommand.DisplayCharacterById(characterId, gameState, ui);
        }

        return CharactersCommand.DisplayCharacters(gameState.PlayerState.Characters, ui);
    }

    private static CommandResult ListSubcommand(
        GameState gameState,
        ConsoleUI ui,
        int indentLevel = 0
    )
    {
        ui.PrintIndentedSection(
            $"Characters List ({gameState.PlayerState.Characters.Count})",
            indentLevel
        );

        for (int i = 0; i < gameState.PlayerState.Characters.Count; i++)
        {
            Character character = gameState.PlayerState.Characters[i];
            bool isLast = i == gameState.PlayerState.Characters.Count - 1;
            ui.PrintIndentedInfo(
                $"{character.GetConfig().Name} ({character.GetConfig().Id})",
                indentLevel + 1,
                isLast
            );
        }

        return CommandResult.Ok();
    }

    private static CommandResult SummarySubcommand(
        GameState gameState,
        ConsoleUI ui,
        int indentLevel = 0
    )
    {
        ui.PrintIndentedSection(
            $"Characters Summary ({gameState.PlayerState.Characters.Count})",
            indentLevel
        );

        return CharactersCommand.DisplayCharacters(
            gameState.PlayerState.Characters,
            ui,
            indentLevel
        );
    }

    public static CommandResult DisplayCharacters(
        List<Character> characters,
        ConsoleUI ui,
        int indentLevel = 0
    )
    {
        for (int i = 0; i < characters.Count; i++)
        {
            bool isLast = i == characters.Count - 1;
            CharactersCommand.DisplayCharacter(characters[i], ui, indentLevel + 1, isLast);
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

        CharactersCommand.DisplayCharacter(character, ui, indentLevel);
        return CommandResult.Ok();
    }

    public static void DisplayCharacter(
        Character character,
        ConsoleUI ui,
        int indentLevel = 0,
        bool isLast = false
    )
    {
        ui.PrintIndentedSection(
            $"{character.GetConfig().Name} ({character.GetConfig().Id})",
            indentLevel,
            isLast
        );

        DisplayStats(character, ui, indentLevel + 1);
        DisplaySkills(character, ui, indentLevel + 1);
    }

    public static void DisplayStats(Character character, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintIndentedSection(
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
        ui.PrintIndentedSection(
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
        ui.PrintIndentedSection(
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

        ui.PrintIndentedSection("Targeter", indentLevel);

        ui.PrintIndentedInfo($"Target Quantity Type: {targeter.TargetQuantity}", indentLevel + 1);
        ui.PrintIndentedInfo(
            $"Allowed Target Types: {string.Join(", ", targeter.AllowedTargets)}",
            indentLevel + 1
        );
        ui.PrintIndentedInfo($"Target Count: {targeter.Count}", indentLevel + 1);
    }

    public static void DisplayUsables(List<IUsable> usables, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintIndentedSection($"Usables ({usables.Count})", indentLevel);

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
        ui.PrintIndentedSection($"{usable.GetConfig().Id}", indentLevel, isLast);

        TargeterConfig usableTargeter = usable.GetConfig().Targeter;
        DisplayTargeter(usableTargeter, ui, indentLevel + 1);
        DisplayEffects(usable.GetConfig().Effects, ui, indentLevel + 1);
    }

    public static void DisplayEffects(List<IEffect> effects, ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintIndentedSection($"Effects ({effects.Count})", indentLevel);

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
        ui.PrintIndentedSection($"{effect.GetConfig().Id}", indentLevel, isLast);

        ui.PrintIndentedInfo($"Type: {effect.GetConfig().Type}", indentLevel + 1);
        ui.PrintIndentedInfo($"Subtype: {effect.GetConfig().Subtype}", indentLevel + 1);
        ui.PrintIndentedInfo($"Variant: {effect.GetConfig().Variant}", indentLevel + 1, isLast);

        if (effect.GetConfig().Type == "Attack")
        {
            ui.PrintIndentedInfo(
                $"Damage: {(effect.GetConfig() as AttackEffectConfig)?.Damage}",
                indentLevel + 1
            );
        }
    }

    protected override List<string> GetExamples()
    {
        return new List<string>
        {
            "characters                    # Show all characters",
            "characters list               # Show all characters",
            "characters summary            # Show a summary of all characters",
            "characters --help                   # Show detailed help for this command",
            "characters --character char_pc_ash        # Show specific character",
            "characters --character=char_pc_ash        # Show specific character",
            "characters -c char_pc_ash        # Show specific character",
            "characters -c=char_pc_ash        # Show specific character",
        };
    }
}
