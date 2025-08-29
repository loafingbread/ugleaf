using GameLogic.Entities;
using PrettyEnough.UI;

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
        // commands["stats"] = new StatsCommand();
        commands["characters"] = new CharactersCommand();
        // commands["modify"] = new ModifyStatCommand();
        // commands["set"] = new SetStatCommand();
        commands["info"] = new InfoCommand();
        commands["clear"] = new ClearCommand();
    }

    public async Task<CommandResult> ProcessCommand(string input, GameState? gameState)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return CommandResult.Error("No command provided");

        var commandName = parts[0].ToLower();
        var args = parts.Skip(1).ToArray();

        if (!commands.ContainsKey(commandName))
            return CommandResult.Error(
                $"Unknown command: {commandName}. Type 'help' for available commands."
            );

        try
        {
            return await commands[commandName].Execute(args, gameState, ui);
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
