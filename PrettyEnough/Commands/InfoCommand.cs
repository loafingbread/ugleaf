using GameLogic.Entities;
using GameLogic.Entities.Characters;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

public class InfoCommand : ICommand
{
    public string Name => "info";
    public string Description => "Show detailed information about the game state";
    public string Usage => "info [subcommand]";

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        return await ProcessArgs(args, gameState, ui);
    }

    private async Task<CommandResult> ProcessArgs(string[] args, GameState? gameState, ConsoleUI ui)
    {
        if (args.Length == 0)
        {
            return DisplaySummary(gameState, ui);
        }

        switch (args[0].ToLower())
        {
            case "characters":
                return await (new CharactersCommand()).Execute(args[1..], gameState, ui);
            default:
                return CommandResult.Error($"Unknown subcommand: {string.Join(" ", args)}");
        }
    }

    public static CommandResult DisplaySummary(GameState? gameState, ConsoleUI ui)
    {
        ui.PrintIndentedSection("ℹ️  Game State Summary", 0);
        DisplayCharactersSummary(gameState, ui, 1);

        return CommandResult.Ok();
    }

    public static CommandResult DisplayCharactersSummary(
        GameState? gameState,
        ConsoleUI ui,
        int indentLevel = 0
    )
    {
        if (gameState?.PlayerState?.Characters == null)
        {
            return CommandResult.Error("No player characters state available");
        }

        ui.PrintIndentedSection(
            $"Available Characters ({gameState.PlayerState.Characters.Count})",
            indentLevel
        );

        foreach (Character character in gameState.PlayerState.Characters)
        {
            ui.PrintIndentedInfo(
                $"{character.GetConfig().Name} ({character.GetConfig().Id})",
                indentLevel + 1
            );
        }

        return CommandResult.Ok();
    }
}
