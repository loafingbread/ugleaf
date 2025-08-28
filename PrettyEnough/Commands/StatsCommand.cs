using GameLogic.Entities.Stats;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class StatsCommand : ICommand
{
    public string Name => "stats";
    public string Description => "Display all current stats in a pretty format";
    public string Usage => "stats [stat_name]";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        if (gameState?.StatBlock == null)
            return CommandResult.Error("No game state available");

        if (args.Length > 0)
        {
            // Show specific stat
            var statName = args[0];
            var stat = gameState.StatBlock.GetStat(statName, StatType.Any);

            if (stat == null)
                return CommandResult.Error($"Stat not found: {statName}");

            DisplayStat(stat, ui);
            return CommandResult.Ok();
        }

        // Show all stats
        ui.PrintSection("📊 Current Stats");

        if (gameState.StatBlock.Stats.Count == 0)
        {
            ui.PrintWarning("No stats available");
            return CommandResult.Ok();
        }

        foreach (var stat in gameState.StatBlock.Stats)
        {
            DisplayStat(stat, ui);
        }

        return await Task.FromResult(CommandResult.Ok());
    }

    private void DisplayStat(Stat stat, ConsoleUI ui)
    {
        var statType = stat.Type switch
        {
            StatType.Value => "📈",
            StatType.Resource => "💧",
            _ => "❓",
        };

        ui.PrintInfo($"{statType} {stat.Metadata.DisplayName} ({stat.Metadata.Name})");
        ui.PrintInfo($"   Current: {stat.CurrentValue}");
        ui.PrintInfo($"   Base: {stat.BaseValue}");
        ui.PrintInfo($"   Type: {stat.Type}");
        ui.PrintInfo($"   Description: {stat.Metadata.Description}");
        ui.PrintInfo($"   Tags: {string.Join(", ", stat.Metadata.Tags)}");

        // Show additional info for resource stats
        if (stat is ResourceStat resourceStat)
        {
            ui.PrintInfo(
                $"   Capacity: {resourceStat.CurrentValue}/{resourceStat.GetConfig().CurrentCapacityCap}"
            );
        }

        ui.PrintInfo("");
    }
}
