using GameLogic.Entities;
using GameLogic.Entities.Stats;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

// public class ModifyStatCommand : ICommand
// {
//     public string Name => "modify";
//     public string Description => "Modify a resource stat by adding/subtracting a value";
//     public string Usage => "modify <stat_name> <amount>";

//     public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
//     {
//         if (gameState?.StatBlock == null)
//             return CommandResult.Error("No game state available");

//         if (args.Length != 2)
//             return CommandResult.Error($"Usage: {Usage}");

//         if (!int.TryParse(args[1], out var amount))
//             return CommandResult.Error("Amount must be a valid integer");

//         var statName = args[0];
//         var modifiedStat = gameState.StatBlock.ModifyResourceStat(statName, amount);

//         if (modifiedStat == null)
//             return CommandResult.Error($"Resource stat not found: {statName}");

//         ui.PrintSection("💧 Resource Stat Modified");
//         ui.PrintInfo($"Stat: {modifiedStat.Metadata.DisplayName} ({statName})");
//         ui.PrintInfo($"Change: {(amount >= 0 ? "+" : "")}{amount}");
//         ui.PrintInfo($"New Value: {modifiedStat.CurrentValue}");
//         ui.PrintInfo(
//             $"Capacity: {modifiedStat.CurrentValue}/{modifiedStat.GetConfig().CurrentCapacityCap}"
//         );

//         return await Task.FromResult(
//             CommandResult.Ok(
//                 $"Modified {statName} by {amount}. New value: {modifiedStat.CurrentValue}"
//             )
//         );
//     }
// }
