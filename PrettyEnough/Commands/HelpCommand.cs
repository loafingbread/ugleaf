using GameLogic.Entities;
using PrettyEnough.UI;
using PrettyEnough.Utils;

namespace PrettyEnough.Commands;

public class HelpCommand : BaseCommand
{
    public override string Name => "help";
    public override string Description => "Show available commands and their usage";
    public override string Usage => "help [command]";

    public override string DetailedHelp => "The help command shows available commands and their usage. "
        + "If a command is provided, it shows detailed help for that command. "
        + "If no command is provided, it shows a list of all commands.";

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
            // Show help for specific command
            var commandName = argParser.PositionalArgs[0].ToLower();
            var command = GetCommandByName(commandName);

            if (command == null)
                return CommandResult.Error($"Unknown command: {commandName}");

            ui.PrintSection($"📖 Help: {command.Name}");
            ui.PrintInfo($"Description: {command.Description}");
            ui.PrintInfo($"Usage: {command.Usage}");

            return CommandResult.Ok();
        }

        // Show all commands
        ui.PrintSection("📖 Available Commands");

        var commands = this.GetAllCommands();
        var headers = new[] { "Command", "Description", "Usage" };
        var rows = commands.Select(cmd => new[] { cmd.Name, cmd.Description, cmd.Usage }).ToArray();

        ui.PrintTable(headers, rows);

        return await Task.FromResult(CommandResult.Ok());
    }

    private ICommand? GetCommandByName(string name)
    {
        return this.GetAllCommands()
            .FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private ICommand[] GetAllCommands()
    {
        return new ICommand[]
        {
            new HelpCommand(),
            new InfoCommand(),
            new ClearCommand(),
            // new ModifyStatCommand(),
            // new SetStatCommand(),
        };
    }
}
