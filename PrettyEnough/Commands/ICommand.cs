using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    string Usage { get; }
    Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui);
}
