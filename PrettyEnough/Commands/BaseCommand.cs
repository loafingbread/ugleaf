using GameLogic.Entities;
using PrettyEnough.UI;

namespace PrettyEnough.Commands;

/// <summary>
/// Base class for all commands that provides common functionality
/// </summary>
public abstract class BaseCommand : ICommand
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Usage { get; }

    /// <summary>
    /// Detailed help text for the command
    /// </summary>
    public virtual string DetailedHelp => Description;

    /// <summary>
    /// Available subcommands for this command
    /// </summary>
    public virtual Dictionary<string, string> Subcommands => new();

    /// <summary>
    /// Available flags for this command
    /// </summary>
    public virtual Dictionary<string, string> Flags =>
        new() { { "--help, -h", "Show detailed help for this command" } };

    public async Task<CommandResult> Execute(string[] args, GameState? gameState, ConsoleUI ui)
    {
        // Check for help flags first
        if (IsHelpRequest(args))
        {
            return DisplayHelp(ui);
        }

        return await ExecuteCommand(args, gameState, ui);
    }

    /// <summary>
    /// Main command execution logic to be implemented by derived classes
    /// </summary>
    protected abstract Task<CommandResult> ExecuteCommand(
        string[] args,
        GameState? gameState,
        ConsoleUI ui
    );

    /// <summary>
    /// Check if the user is requesting help
    /// </summary>
    protected bool IsHelpRequest(string[] args)
    {
        return args.Length > 0 && (args[0] == "--help" || args[0] == "-h" || args[0] == "help");
    }

    /// <summary>
    /// Display comprehensive help for this command
    /// </summary>
    protected virtual CommandResult DisplayHelp(ConsoleUI ui)
    {
        ui.PrintSection($"📖 {Name.ToUpper()} Command Help");

        // Description
        ui.PrintInfo(Description);
        ui.PrintIndentedNewLine(1);

        // Usage
        ui.PrintIndentedSection("Usage", 1);
        ui.PrintIndentedInfo(Usage, 2);
        ui.PrintIndentedNewLine(1);

        // Subcommands
        if (Subcommands.Any())
        {
            ui.PrintIndentedSection("Subcommands", 1);
            foreach (var subcommand in Subcommands)
            {
                ui.PrintIndentedInfo($"{subcommand.Key}: {subcommand.Value}", 2);
            }
            ui.PrintIndentedNewLine(1);
        }

        // Flags
        if (Flags.Any())
        {
            ui.PrintIndentedSection("Flags", 1);
            foreach (var flag in Flags)
            {
                ui.PrintIndentedInfo($"{flag.Key}: {flag.Value}", 2);
            }
            ui.PrintIndentedNewLine(1);
        }

        // Detailed help
        if (!string.IsNullOrEmpty(DetailedHelp) && DetailedHelp != Description)
        {
            ui.PrintIndentedSection("Details", 1);
            ui.PrintIndentedInfo(DetailedHelp, 2);
            ui.PrintIndentedNewLine(1);
        }

        // Examples
        var examples = GetExamples();
        if (examples.Any())
        {
            ui.PrintIndentedSection("Examples", 1);
            foreach (var example in examples)
            {
                ui.PrintIndentedInfo(example, 2);
            }
        }

        return CommandResult.Ok();
    }

    /// <summary>
    /// Get example usage for this command
    /// </summary>
    protected virtual List<string> GetExamples()
    {
        return new List<string>();
    }

    /// <summary>
    /// Parse command line arguments and handle common patterns
    /// </summary>
    protected (string[] positionalArgs, Dictionary<string, string> flags) ParseArgs(string[] args)
    {
        var positionalArgs = new List<string>();
        var flags = new Dictionary<string, string>();

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.StartsWith("--"))
            {
                // Long flag: --flag=value or --flag value
                var flagName = arg.Substring(2);
                if (flagName.Contains('='))
                {
                    var parts = flagName.Split('=', 2);
                    flags[parts[0]] = parts[1];
                }
                else if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                {
                    flags[flagName] = args[i + 1];
                    i++; // Skip the next argument
                }
                else
                {
                    flags[flagName] = "true";
                }
            }
            else if (arg.StartsWith("-") && arg.Length > 1)
            {
                // Short flag: -f=value or -f value
                var flagName = arg.Substring(1);
                if (flagName.Contains('='))
                {
                    var parts = flagName.Split('=', 2);
                    flags[parts[0]] = parts[1];
                }
                else if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                {
                    flags[flagName] = args[i + 1];
                    i++; // Skip the next argument
                }
                else
                {
                    flags[flagName] = "true";
                }
            }
            else
            {
                positionalArgs.Add(arg);
            }
        }

        return (positionalArgs.ToArray(), flags);
    }
}
