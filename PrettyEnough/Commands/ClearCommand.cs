using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class ClearCommand : ICommand
{
    public string Name => "clear";
    public string Description => "Clear the console screen";
    public string Usage => "clear";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        await Task.Run(() => Console.Clear());
        ui.PrintWelcome();
        return CommandResult.Ok("Console cleared");
    }
}
