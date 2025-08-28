using GameLogic.Entities.Stats;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class SetStatCommand : ICommand
{
    public string Name => "set";
    public string Description => "Set a resource stat to a specific value";
    public string Usage => "set <stat_name> <value>";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        if (gameState?.StatBlock == null)
            return CommandResult.Error("No game state available");

        if (args.Length != 2)
            return CommandResult.Error($"Usage: {Usage}");

        if (!int.TryParse(args[1], out var value))
            return CommandResult.Error("Value must be a valid integer");

        var statName = args[0];
        var setStat = gameState.StatBlock.SetResourceStat(statName, value);

        if (setStat == null)
            return CommandResult.Error($"Resource stat not found: {statName}");

        ui.PrintSection("🎯 Resource Stat Set");
        ui.PrintInfo($"Stat: {setStat.Metadata.DisplayName} ({statName})");
        ui.PrintInfo($"New Value: {setStat.CurrentValue}");
        ui.PrintInfo($"Capacity: {setStat.CurrentValue}/{setStat.GetConfig().CurrentCapacityCap}");

        return await Task.FromResult(CommandResult.Ok($"Set {statName} to {value}"));
    }
}
