using GameLogic.Entities;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Stats;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class InfoCommand : BaseCommand
{
    public override string Name => "info";
    public override string Description =>
        "Show detailed information about the game state and its components";
    public override string Usage => "info                    # Show game state summary\ninfo [subcommand] [options] # Show specific information";

    public override string DetailedHelp =>
        "The info command provides detailed information about the current game state. "
        + "When run without arguments, it shows a summary of all available information. "
        + "Use subcommands to focus on specific aspects of the game, such as characters or stats.";

    public override Dictionary<string, string> Subcommands =>
        new()
        {
            { "default", "Show a summary of the game state (default when no subcommand given)" },
            { "characters", "Show detailed information about all characters" },
        };

    public override Dictionary<string, string> Flags =>
        new()
        {
            { "--help, -h", "Show detailed help for this command" },
            { "--verbose, -v", "Show more detailed information" },
            { "--format", "Output format (text, json, table)" },
        };

    protected override async Task<CommandResult> ExecuteCommand(
        string[] args,
        GameState? gameState,
        ConsoleUI ui
    )
    {
        var (positionalArgs, flags) = ParseArgs(args);

        if (positionalArgs.Length == 0)
        {
            return DisplaySummary(gameState, ui, flags);
        }

        var subcommand = positionalArgs[0].ToLower();
        var subcommandArgs = positionalArgs.Skip(1).ToArray();

        return subcommand switch
        {
            "characters" => await HandleCharactersSubcommand(subcommandArgs, gameState, ui, flags),
            "default" => DisplaySummary(gameState, ui, flags),
            "summary" => DisplaySummary(gameState, ui, flags),
            _ => CommandResult.Error(
                $"Unknown subcommand: {subcommand}. Use 'info --help' for available options."
            ),
        };
    }

    private async Task<CommandResult> HandleCharactersSubcommand(
        string[] args,
        GameState? gameState,
        ConsoleUI ui,
        Dictionary<string, string> flags
    )
    {
        // Check if the user wants help for the characters subcommand
        if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
        {
            return DisplayCharactersHelp(ui);
        }

        // Delegate to CharactersCommand
        return await new CharactersCommand().Execute(args, gameState, ui);
    }

    private CommandResult DisplayCharactersHelp(ConsoleUI ui)
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

    public static CommandResult DisplaySummary(
        GameState? gameState,
        ConsoleUI ui,
        Dictionary<string, string> flags = null
    )
    {
        flags ??= new Dictionary<string, string>();
        var verbose = flags.ContainsKey("verbose") || flags.ContainsKey("v");

        ui.PrintSection("ℹ️  Game State Summary");

        if (gameState == null)
        {
            ui.PrintWarning("No game state available");
            return CommandResult.Ok();
        }

        DisplayCharactersSummary(gameState, ui, verbose);

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

    public static CommandResult DisplayCharactersSummary(
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
            var characterInfo = $"{character.GetConfig().Name} ({character.GetConfig().Id})";

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
            "info                    # Show game state summary (default)",
            "info default            # Same as 'info' - show summary",
            "info --verbose          # Show detailed summary",
            "info characters         # Show all characters",
            "info characters alice   # Show specific character",
            "info --help             # Show this help message",
        };
    }
}
