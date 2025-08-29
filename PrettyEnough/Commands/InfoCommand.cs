using GameLogic.Entities;
using GameLogic.Entities.Stats;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class InfoCommand : ICommand
{
    public string Name => "info";
    public string Description => "Show detailed information about the game state";
    public string Usage => "info";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        if (gameState?.PlayerState?.Characters == null)
            return CommandResult.Error("No player characters state available");

        ui.PrintSection("ℹ️  Game State Information");

        // Characters info
        ui.PrintInfo($"📦 Characters: {gameState.PlayerState.Characters.Count} characters loaded");

        // Available characters
        ui.PrintInfo("");
        ui.PrintInfo("📋 Available Characters:");
        foreach (var character in gameState.PlayerState.Characters)
        {
            ui.PrintInfo($"   {character.GetConfig().Name} ({character.GetConfig().Id})");
        }

        return await Task.FromResult(CommandResult.Ok());
    }
}
