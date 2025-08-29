using GameLogic.Entities;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class ExampleCommand : BaseCommand
{
    public override string Name => "example";
    public override string Description => "Example command demonstrating the help system";
    public override string Usage => "example [subcommand] [options]";

    public override string DetailedHelp =>
        "This is an example command that demonstrates how the help system works. "
        + "It shows subcommands, flags, and provides examples of usage patterns.";

    public override Dictionary<string, string> Subcommands =>
        new()
        {
            { "demo", "Run a demonstration" },
            { "test", "Run tests" },
            { "show", "Show information" },
        };

    public override Dictionary<string, string> Flags =>
        new()
        {
            { "--help, -h", "Show detailed help for this command" },
            { "--verbose, -v", "Show verbose output" },
            { "--count", "Number of items to process" },
            { "--format", "Output format (text, json, xml)" },
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
            return DisplayDefault(ui, flags);
        }

        var subcommand = positionalArgs[0].ToLower();
        var subcommandArgs = positionalArgs.Skip(1).ToArray();

        return subcommand switch
        {
            "demo" => DisplayDemo(ui, subcommandArgs, flags),
            "test" => DisplayTest(ui, subcommandArgs, flags),
            "show" => DisplayShow(ui, subcommandArgs, flags),
            _ => CommandResult.Error(
                $"Unknown subcommand: {subcommand}. Use 'example --help' for available options."
            ),
        };
    }

    private CommandResult DisplayDefault(ConsoleUI ui, Dictionary<string, string> flags)
    {
        var verbose = flags.ContainsKey("verbose") || flags.ContainsKey("v");

        ui.PrintSection("🎯 Example Command");
        ui.PrintInfo("This is the default behavior of the example command.");

        if (verbose)
        {
            ui.PrintIndentedSection("Verbose Information", 1);
            ui.PrintIndentedInfo("Running in verbose mode", 2);
            ui.PrintIndentedInfo("Flags detected: " + string.Join(", ", flags.Keys), 2);
        }

        return CommandResult.Ok();
    }

    private CommandResult DisplayDemo(ConsoleUI ui, string[] args, Dictionary<string, string> flags)
    {
        ui.PrintSection("🎭 Demo Mode");
        ui.PrintInfo("Running demonstration...");

        var count = flags.ContainsKey("count") ? int.Parse(flags["count"]) : 3;

        for (int i = 1; i <= count; i++)
        {
            ui.PrintIndentedInfo($"Demo item {i}", 1);
        }

        return CommandResult.Ok();
    }

    private CommandResult DisplayTest(ConsoleUI ui, string[] args, Dictionary<string, string> flags)
    {
        ui.PrintSection("🧪 Test Mode");
        ui.PrintInfo("Running tests...");
        ui.PrintSuccess("All tests passed!");

        return CommandResult.Ok();
    }

    private CommandResult DisplayShow(ConsoleUI ui, string[] args, Dictionary<string, string> flags)
    {
        ui.PrintSection("📊 Show Mode");

        var format = flags.ContainsKey("format") ? flags["format"] : "text";
        ui.PrintInfo($"Showing information in {format} format");

        return CommandResult.Ok();
    }

    protected override List<string> GetExamples()
    {
        return new List<string>
        {
            "example                    # Show default behavior",
            "example --verbose          # Show verbose output",
            "example demo               # Run demonstration",
            "example demo --count=5     # Run demo with 5 items",
            "example show --format=json # Show in JSON format",
            "example --help             # Show this help message",
        };
    }
}
