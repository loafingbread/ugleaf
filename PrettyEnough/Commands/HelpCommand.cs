using GameLogic.Entities;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class HelpCommand : ICommand
{
    public string Name => "help";
    public string Description => "Show available commands and their usage";
    public string Usage => "help [command]";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        if (args.Length > 0)
        {
            // Show help for specific command
            var commandName = args[0].ToLower();
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

        var commands = GetAllCommands();
        var headers = new[] { "Command", "Description", "Usage" };
        var rows = commands.Select(cmd => new[] { cmd.Name, cmd.Description, cmd.Usage }).ToArray();

        ui.PrintTable(headers, rows);

        return await Task.FromResult(CommandResult.Ok());
    }

    private ICommand? GetCommandByName(string name)
    {
        return GetAllCommands()
            .FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private ICommand[] GetAllCommands()
    {
        return new ICommand[]
        {
            new HelpCommand(),
            new CharactersCommand(),
            // new ModifyStatCommand(),
            // new SetStatCommand(),
            new InfoCommand(),
            new ClearCommand(),
        };
    }
}
