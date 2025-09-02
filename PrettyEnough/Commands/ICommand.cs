using GameLogic.Entities;
using PrettyEnough.Utils;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    string Usage { get; }
    Task<CommandResult> Execute(ArgParser argParser, GameState? gameState, ConsoleUI ui);
}
