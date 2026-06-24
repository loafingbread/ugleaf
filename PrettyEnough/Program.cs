namespace PrettyEnough;

using GameLogic.Config;
using GameLogic.Entities;
using GameLogic.Entities.Stats;
using PrettyEnough.Commands;
using PrettyEnough.UI;

class Program
{
    private static GameState? gameState;
    private static readonly CommandProcessor commandProcessor = new();
    private static readonly ConsoleUI ui = new();

    static async Task Main(string[] args)
    {
        ui.PrintWelcome();

        try
        {
            await InitializeGame();
            await RunGameLoop();
        }
        catch (Exception ex)
        {
            ui.PrintError($"Fatal error: {ex.Message}");
            ui.PrintError($"Stack trace: {ex.StackTrace}");
        }
    }

    private static async Task InitializeGame()
    {
        ui.PrintInfo("🎮 Initializing PrettyEnough...");

        try
        {
            // Load test stat block
            var gameStateRecord = await Task.Run(() =>
                JsonConfigLoader.LoadFromFile<GameStateRecord>(ConfigPaths.GameState.Simple)
            );

            gameState = GameStateFactory.CreateFromRecord(gameStateRecord);
            if (gameState == null)
                throw new Exception("Game state failed to be created");

            ui.PrintSuccess("✅ Game initialized successfully!");
            ui.PrintIndentedInfo($"Loaded {gameState.PlayerState.Characters.Count} player(s), {gameState.EnemyState.Characters.Count} enemy/enemies", 0);

            ui.PrintSection("Players");
            CharactersCommand.DisplayCharacters(gameState.PlayerState.Characters, ui);
            ui.PrintSection("Enemies");
            CharactersCommand.DisplayCharacters(gameState.EnemyState.Characters, ui);
        }
        catch (Exception ex)
        {
            ui.PrintError($"❌ Failed to initialize game: {ex.Message}");
            throw;
        }
    }

    private static async Task RunGameLoop()
    {
        ui.PrintInfo("🚀 PrettyEnough is ready! Type 'help' for available commands.");

        while (true)
        {
            try
            {
                ui.PrintPrompt();
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                if (input.ToLower() == "exit" || input.ToLower() == "quit")
                {
                    ui.PrintInfo("👋 Goodbye!");
                    break;
                }

                await ProcessCommand(input);
            }
            catch (Exception ex)
            {
                ui.PrintError($"❌ Error: {ex.Message}");
            }
        }
    }

    private static async Task ProcessCommand(string input)
    {
        var result = await commandProcessor.ProcessCommand(input, gameState);

        if (result.Success)
        {
            if (!string.IsNullOrEmpty(result.Message))
                ui.PrintSuccess(result.Message);
        }
        else
        {
            ui.PrintError($"❌ {result.Message}");
        }
    }
}
