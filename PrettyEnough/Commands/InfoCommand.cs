using GameLogic.Entities;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Stats;
using PrettyEnough.UI;
using PrettyEnough.Utils;

namespace PrettyEnough.Commands;

public class InfoCommand : BaseCommand
{
    public override string Name => "info";
    public override string Description =>
        "Show detailed information about the game state and its components";
    public override string Usage => "info [subcommand] [options]";

    public override string DetailedHelp =>
        "The info command provides detailed information about the current game state. "
        + "When run without arguments, it shows a summary of all available information. "
        + "Use subcommands to focus on specific aspects of the game, such as characters or stats.";

    public override Dictionary<string, string> Subcommands =>
        new()
        {
            { "summary", "Show a summary of the game state (default when no subcommand given)" },
            { "characters", "Show detailed information about all characters" },
        };

    public override Dictionary<string, string> Flags =>
        new()
        {
            { "--help, -h", "Show detailed help for this command" },
            { "--verbose, -v", "Show more detailed information" },
        };

    protected override async Task<CommandResult> ExecuteCommand(
        ArgParser argParser,
        GameState? gameState,
        ConsoleUI ui
    )
    {
        if (argParser.PositionalArgs.Count == 0)
        {
            return InfoCommand.DefaultSubcommand(gameState, ui, argParser);
        }

        string subcommand = argParser.PositionalArgs[0].ToLower();
        argParser.PositionalArgs.RemoveAt(0);

        return subcommand switch
        {
            "characters" => await InfoCommand.CharactersSubcommand(gameState, ui, argParser),
            "summary" => InfoCommand.SummarySubcommand(gameState, ui, argParser),
            _ => CommandResult.Error(
                $"Unknown subcommand: {subcommand}. Use 'info --help' for available options."
            ),
        };
    }

    private static CommandResult DefaultSubcommand(
        GameState? gameState,
        ConsoleUI ui,
        ArgParser argParser
    )
    {
        return SummarySubcommand(gameState, ui, argParser);
    }

    private static async Task<CommandResult> CharactersSubcommand(
        GameState? gameState,
        ConsoleUI ui,
        ArgParser argParser
    )
    {
        // Delegate to CharactersCommand
        return await (new CharactersCommand()).Execute(argParser, gameState, ui);
    }

    private static CommandResult HelpSubcommand(ConsoleUI ui)
    {
        ui.PrintSection("📖 INFO CHARACTERS Subcommand Help");

        ui.PrintInfo("Show detailed information about characters in the game state.");

        ui.PrintIndentedSection("Usage", 1);
        ui.PrintIndentedInfo("info characters [character_id] [options]", 2);

        ui.PrintIndentedSection("Arguments", 1);
        ui.PrintIndentedInfo("character_id: Show details for a specific character (optional)", 2);

        ui.PrintIndentedSection("Examples", 1);
        ui.PrintIndentedInfo("info characters", 2);
        ui.PrintIndentedInfo("info characters char_alice", 2);
        ui.PrintIndentedInfo("info characters --verbose", 2);

        return CommandResult.Ok();
    }

    private static CommandResult SummarySubcommand(
        GameState? gameState,
        ConsoleUI ui,
        ArgParser argParser
    )
    {
        ui.PrintSection("ℹ️  Game State Summary");

        if (gameState == null)
        {
            ui.PrintWarning("No game state available");
            return CommandResult.Ok();
        }

        bool verbose = argParser.HasFlag("verbose", "v");
        InfoCommand.DisplayCharactersSummary(gameState, ui, verbose);

        if (verbose)
        {
            // Add more detailed information in verbose mode
            ui.PrintIndentedSection("Additional Information", 1);
            ui.PrintIndentedInfo("Game state loaded successfully", 2);
            ui.PrintIndentedInfo(
                $"Total entities: {gameState.PlayerState?.Characters?.Count ?? 0}",
                2
            );
        }

        return CommandResult.Ok();
    }

    private static CommandResult DisplayCharactersSummary(
        GameState? gameState,
        ConsoleUI ui,
        bool verbose = false,
        int indentLevel = 0
    )
    {
        if (gameState?.PlayerState?.Characters == null)
        {
            ui.PrintWarning("No player characters available");
            return CommandResult.Ok();
        }

        ui.PrintIndentedSection(
            $"Available Characters ({gameState.PlayerState.Characters.Count})",
            indentLevel
        );

        foreach (Character character in gameState.PlayerState.Characters)
        {
            var characterInfo = $"{character.Name} ({character.InstanceId.ToString()})";

            if (verbose)
            {
                // Add more details in verbose mode
                var healthStat =
                    character.Stats.GetStat("resource_stat_health", StatType.Resource)
                    as ResourceStat;
                var healthInfo =
                    healthStat != null
                        ? $" - Health: {healthStat.CurrentValue}/{healthStat.CurrentCapacity}"
                        : "";
                characterInfo += healthInfo;
            }

            ui.PrintIndentedInfo(characterInfo, indentLevel + 1);
        }

        return CommandResult.Ok();
    }

    protected override List<string> GetExamples()
    {
        return new List<string>
        {
            "info                         # Show game state summary (default)",
            "info summary                 # Show game state summary",
            "info characters              # Show all characters",
            "info --verbose               # Show detailed summary",
            "info --help                  # Show this help message",
        };
    }
}
