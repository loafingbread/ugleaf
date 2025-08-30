using GameLogic.Entities;
using PrettyEnough.UI;
using PrettyEnough.Utils;

namespace PrettyEnough.Commands;

public class ClearCommand : BaseCommand
{
    public override string Name => "clear";
    public override string Description => "Clear the console screen";
    public override string Usage => "clear";

    public override string DetailedHelp => "The clear command clears the console screen";

    public override Dictionary<string, string> Subcommands => new() { };

    public override Dictionary<string, string> Flags => new() { };

    protected override async Task<CommandResult> ExecuteCommand(
        ArgParser argParser,
        GameState? gameState,
        ConsoleUI ui
    )
    {
        if (argParser.PositionalArgs.Count > 0)
        {
            return CommandResult.Error("Clear command does not take any positional arguments");
        }

        await Task.Run(() => Console.Clear());
        ui.PrintWelcome();

        return CommandResult.Ok("Console cleared");
    }
}
