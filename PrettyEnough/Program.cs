namespace PrettyEnough;

using GameLogic.Config;
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
            var statBlockRecord = await Task.Run(() =>
                JsonConfigLoader.LoadFromFile<StatBlockRecord>(ConfigPaths.Stat.TestStatBlock)
            );

            gameState = new GameState
            {
                StatBlock = StatFactory.CreateStatBlockFromRecord(statBlockRecord),
            };

            ui.PrintSuccess("✅ Game initialized successfully!");
            ui.PrintInfo($"📊 Loaded {gameState.StatBlock.Stats.Count} stats");

            // Display initial stats
            DisplayStats();
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

    private static void DisplayStats()
    {
        if (gameState?.StatBlock == null)
            return;

        ui.PrintSection("📊 Current Stats");

        foreach (var stat in gameState.StatBlock.Stats)
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
            ui.PrintInfo($"   Description: {stat.Metadata.Description}");
            ui.PrintInfo($"   Tags: {string.Join(", ", stat.Metadata.Tags)}");
            ui.PrintInfo("");
        }
    }
}
