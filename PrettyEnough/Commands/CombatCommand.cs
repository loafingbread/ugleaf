namespace PrettyEnough.Commands;

using GameLogic.Combat.TurnBased;
using GameLogic.Entities;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Events;
using GameLogic.Events.Categories;
using PrettyEnough.UI;
using PrettyEnough.Utils;

public class CombatCommand : BaseCommand
{
    public override string Name => "combat";
    public override string Description => "Run a turn-based combat simulation";
    public override string Usage => "combat <subcommand> [options]";

    public override Dictionary<string, string> Subcommands =>
        new()
        {
            { "start", "Start a new combat with the loaded players vs enemies" },
            { "status", "Show current phase, turn order, and HP of all combatants" },
            { "select <skill#> <target#>", "Select a skill and target for the current player's turn" },
            { "end", "End the current combat" },
        };

    public override Dictionary<string, string> Flags =>
        new() { { "--help, -h", "Show detailed help for this command" } };

    protected override async Task<CommandResult> ExecuteCommand(
        ArgParser argParser,
        GameState? gameState,
        ConsoleUI ui
    )
    {
        if (gameState == null)
            return CommandResult.Error("No game state loaded");

        if (argParser.PositionalArgs.Count == 0)
            return StatusSubcommand(gameState, ui);

        string subcommand = argParser.PositionalArgs[0].ToLower();
        argParser.PositionalArgs.RemoveAt(0);

        return await Task.FromResult(subcommand switch
        {
            "start" => StartSubcommand(gameState, ui),
            "status" => StatusSubcommand(gameState, ui),
            "select" => SelectSubcommand(gameState, ui, argParser),
            "end" => EndSubcommand(gameState, ui),
            _ => CommandResult.Error(
                $"Unknown subcommand '{subcommand}'. Use 'combat --help' for available options."
            ),
        });
    }

    private static CommandResult StartSubcommand(GameState gameState, ConsoleUI ui)
    {
        if (gameState.ActiveCombat != null)
            return CommandResult.Error("A combat is already active. Use 'combat end' to stop it first.");

        if (gameState.PlayerState.Characters.Count == 0)
            return CommandResult.Error("No player characters loaded.");

        if (gameState.EnemyState.Characters.Count == 0)
            return CommandResult.Error("No enemies loaded.");

        Combat combat = new(
            [.. gameState.PlayerState.Characters],
            [.. gameState.EnemyState.Characters]
        );

        combat.State.EventBus.Subscribe<CombatPhaseChangeEvent, BuiltInEventCategory>(e =>
        {
            ui.PrintInfo($"[Phase] {e.Action} → {e.Phase}");
            return Task.CompletedTask;
        });

        combat.State.EventBus.Subscribe<SkillUseEvent, BuiltInEventCategory>(e =>
        {
            ui.PrintInfo($"[Skill] {e.User} uses {e.Skill}");
            return Task.CompletedTask;
        });

        gameState.ActiveCombat = combat;

        PlayUntilInput(combat);

        if (combat.IsCombatOver())
        {
            PrintCombatOver(ui);
            gameState.ActiveCombat = null;
            return CommandResult.Ok();
        }

        return StatusSubcommand(gameState, ui);
    }

    private static CommandResult StatusSubcommand(GameState gameState, ConsoleUI ui)
    {
        if (gameState.ActiveCombat == null)
            return CommandResult.Error("No active combat. Use 'combat start' to begin.");

        Combat combat = gameState.ActiveCombat;

        ui.PrintSection("⚔️  Combat Status");
        ui.PrintInfo($"Phase: {combat.State.Phase}");

        Character current = combat.State.Queue.GetCurrentTurn();
        ui.PrintInfo($"Current turn: {current.Name}");

        ui.PrintIndentedSection("Players", 0);
        for (int i = 0; i < combat.State.Queue.Players.Count; i++)
        {
            Character c = combat.State.Queue.Players[i];
            ui.PrintIndentedInfo($"[{i}] {c.Name} ({c.Id})  HP: {GetHp(c)}", 1);
        }

        ui.PrintIndentedSection("Enemies", 0);
        for (int i = 0; i < combat.State.Queue.Enemies.Count; i++)
        {
            Character c = combat.State.Queue.Enemies[i];
            ui.PrintIndentedInfo($"[{i}] {c.Name} ({c.Id})  HP: {GetHp(c)}", 1);
        }

        if (combat.State.Phase == EPhase.AwaitPlayerSelectCommand)
        {
            ui.PrintIndentedSection($"{current.Name}'s Skills", 0);
            for (int i = 0; i < current.Skills.Count; i++)
            {
                Skill skill = current.Skills[i];
                ui.PrintIndentedInfo($"[{i}] {skill.Name}", 1);
            }
            ui.PrintInfo("Use: combat select <skill#> <target#>");
        }

        return CommandResult.Ok();
    }

    private static CommandResult SelectSubcommand(GameState gameState, ConsoleUI ui, ArgParser argParser)
    {
        if (gameState.ActiveCombat == null)
            return CommandResult.Error("No active combat. Use 'combat start' to begin.");

        Combat combat = gameState.ActiveCombat;

        if (combat.State.Phase != EPhase.AwaitPlayerSelectCommand)
            return CommandResult.Error($"Cannot select now — current phase is {combat.State.Phase}.");

        if (argParser.PositionalArgs.Count < 2)
            return CommandResult.Error("Usage: combat select <skill#> <target#>");

        if (!int.TryParse(argParser.PositionalArgs[0], out int skillIdx))
            return CommandResult.Error("skill# must be a number.");
        if (!int.TryParse(argParser.PositionalArgs[1], out int targetIdx))
            return CommandResult.Error("target# must be a number.");

        Character current = combat.State.Queue.GetCurrentTurn();

        if (skillIdx < 0 || skillIdx >= current.Skills.Count)
            return CommandResult.Error($"Skill index {skillIdx} out of range (0–{current.Skills.Count - 1}).");

        IReadOnlyList<Character> enemies = combat.State.Queue.Enemies;
        if (targetIdx < 0 || targetIdx >= enemies.Count)
            return CommandResult.Error($"Target index {targetIdx} out of range (0–{enemies.Count - 1}).");

        Skill skill = current.Skills[skillIdx];
        Character target = enemies[targetIdx];

        ui.PrintInfo($"→ {current.Name} uses {skill.Name} on {target.Name}");

        combat.PlayerSelectCommand(skill, [target]);
        PlayUntilInput(combat);

        if (combat.IsCombatOver())
        {
            PrintCombatOver(ui);
            gameState.ActiveCombat = null;
            return CommandResult.Ok();
        }

        return StatusSubcommand(gameState, ui);
    }

    private static CommandResult EndSubcommand(GameState gameState, ConsoleUI ui)
    {
        if (gameState.ActiveCombat == null)
            return CommandResult.Error("No active combat.");

        gameState.ActiveCombat = null;
        ui.PrintInfo("Combat ended.");
        return CommandResult.Ok();
    }

    private static void PlayUntilInput(Combat combat)
    {
        int safety = 0;
        while (combat.State.Phase != EPhase.AwaitPlayerSelectCommand
            && combat.State.Phase != EPhase.CombatEnd
            && safety < 200)
        {
            combat.Play();
            safety++;
        }
    }

    private static void PrintCombatOver(ConsoleUI ui)
    {
        ui.PrintSuccess("Combat over!");
    }

    private static string GetHp(Character c)
    {
        ResourceStat? hp = c.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
        return hp != null ? $"{hp.CurrentValue}/{hp.CurrentCapacity}" : "?";
    }

    protected override List<string> GetExamples() =>
        [
            "combat start            # Begin combat with loaded players vs enemies",
            "combat status           # Show current phase and HP",
            "combat select 0 0       # Use skill 0 on enemy 0",
            "combat end              # Abort current combat",
        ];
}
