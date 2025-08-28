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
        if (gameState?.StatBlock == null)
            return CommandResult.Error("No game state available");

        ui.PrintSection("ℹ️  Game State Information");

        // Stat block info
        ui.PrintInfo($"📦 Stat Block: {gameState.StatBlock.Stats.Count} stats loaded");

        // Stat breakdown
        var valueStats = gameState.StatBlock.Stats.Where(s => s.Type == StatType.Value).Count();
        var resourceStats = gameState
            .StatBlock.Stats.Where(s => s.Type == StatType.Resource)
            .Count();

        ui.PrintInfo($"📈 Value Stats: {valueStats}");
        ui.PrintInfo($"💧 Resource Stats: {resourceStats}");

        // Available stat names
        ui.PrintInfo("");
        ui.PrintInfo("📋 Available Stats:");
        foreach (var stat in gameState.StatBlock.Stats)
        {
            var typeIcon = stat.Type switch
            {
                StatType.Value => "📈",
                StatType.Resource => "💧",
                _ => "❓",
            };
            ui.PrintInfo($"   {typeIcon} {stat.Metadata.Name} ({stat.Metadata.DisplayName})");
        }

        return await Task.FromResult(CommandResult.Ok());
    }
}
