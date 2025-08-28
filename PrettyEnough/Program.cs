namespace PrettyEnough;

using GameLogic.Config;
using GameLogic.Entities.Stats;
using PrettyEnough.UI;

class Program
{
    private static GameState? gameState;
    private static readonly ConsoleUI ui = new();

    static async Task Main(string[] args)
    {
        ui.PrintWelcome();

        try
        {
            await InitializeGame();
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
        }
        catch (Exception ex)
        {
            ui.PrintError($"❌ Failed to initialize game: {ex.Message}");
            throw;
        }
    }
}
