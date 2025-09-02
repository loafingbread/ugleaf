using GameLogic.Entities;
using PrettyEnough.UI;
using PrettyEnough.Utils;

namespace PrettyEnough.Commands;

public class CommandProcessor
{
    private readonly Dictionary<string, ICommand> commands = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConsoleUI ui = new();

    public CommandProcessor()
    {
        RegisterCommands();
    }

    private void RegisterCommands()
    {
        commands["help"] = new HelpCommand();
        commands["info"] = new InfoCommand();
        commands["clear"] = new ClearCommand();
        // commands["stats"] = new StatsCommand();
        // commands["modify"] = new ModifyStatCommand();
        // commands["set"] = new SetStatCommand();
    }

    public async Task<CommandResult> ProcessCommand(string input, GameState? gameState)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return CommandResult.Error("No command provided");

        var commandName = parts[0].ToLower();
        var args = parts.Skip(1).ToArray();

        if (!this.commands.ContainsKey(commandName))
            return CommandResult.Error(
                $"Unknown command: {commandName}. Type 'help' for available commands."
            );

        try
        {
            ArgParser argParser = new ArgParser(args);
            argParser.Parse();

            return await this.commands[commandName].Execute(argParser, gameState, ui);
        }
        catch (Exception ex)
        {
            return CommandResult.Error($"Command execution failed: {ex.Message}");
        }
    }

    public IEnumerable<string> GetAvailableCommands()
    {
        return commands.Keys.OrderBy(k => k);
    }
}
