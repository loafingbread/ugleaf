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
            ui.PrintInfo($"📊 Loaded {gameState.PlayerState.Characters.Count} characters");

            // Display initial player state
            CharactersCommand.DisplayCharacters(gameState.PlayerState.Characters, ui);
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

    private static void DisplayPlayerState()
    {
        if (gameState?.PlayerState == null)
            return;

        ui.PrintSection("📊 Current Player State");

        foreach (var character in gameState.PlayerState.Characters)
        {
            ui.PrintInfo($"{character.GetConfig().Name} ({character.GetConfig().Id})");

            Stat strengthStat = character.Stats.GetStat("value_stat_strength", StatType.Value);
            ui.PrintInfo($"   {strengthStat?.Metadata.DisplayName}: {strengthStat?.CurrentValue}");

            Stat agilityStat = character.Stats.GetStat("value_stat_agility", StatType.Value);
            ui.PrintInfo($"   {agilityStat?.Metadata.DisplayName}: {agilityStat?.CurrentValue}");

            Stat healthStat = character.Stats.GetStat("resource_stat_health", StatType.Resource);
            ui.PrintInfo($"   {healthStat?.Metadata.DisplayName}: {healthStat?.CurrentValue}");

            ui.PrintInfo("");
        }
    }
}
