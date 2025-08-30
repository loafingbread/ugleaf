namespace PrettyEnough.Commands;

using GameLogic.Entities;
using PrettyEnough.UI;
using PrettyEnough.Utils;

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

    public async Task<CommandResult> Execute(
        ArgParser argParser,
        GameState? gameState,
        ConsoleUI ui
    )
    {
        CommandResult? helpResult = HelpCommand(ui, argParser);
        if (helpResult != null)
        {
            return helpResult;
        }

        return await ExecuteCommand(argParser, gameState, ui);
    }

    private CommandResult? HelpCommand(ConsoleUI ui, ArgParser argParser)
    {
        bool isHelpFlagForCurrentCommand =
            argParser.PositionalArgs.Count == 0 && argParser.HasFlag("help", "h");
        if (isHelpFlagForCurrentCommand)
        {
            return DisplayHelp(ui);
        }

        bool isHelpFlagSubcommand = argParser.PositionalArgs.Count > 0 && argParser.PositionalArgs[0] == "help";
        if (isHelpFlagSubcommand)
        {
            return DisplayHelp(ui);
        }

        return null;
    }

    /// <summary>
    /// Main command execution logic to be implemented by derived classes
    /// </summary>
    protected abstract Task<CommandResult> ExecuteCommand(
        ArgParser argParser,
        GameState? gameState,
        ConsoleUI ui
    );

    /// <summary>
    /// Check if the user is requesting help
    /// </summary>
    protected bool IsHelpRequest(string[] args)
    {
        return (new ArgParser(args)).IsHelpRequest();
    }

    /// <summary>
    /// Display comprehensive help for this command
    /// </summary>
    protected virtual CommandResult DisplayHelp(ConsoleUI ui)
    {
        ui.PrintIndentedSection($"📖 {Name.ToUpper()} Command Help", 0);

        this.DisplayDescription(ui, 1);
        ui.PrintIndentedNewLine(1);

        this.DisplayUsage(ui, 1);
        ui.PrintIndentedNewLine(1);

        // Subcommands
        this.DisplaySubcommands(ui, 1);
        ui.PrintIndentedNewLine(1);
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

    protected virtual void DisplayDescription(ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintIndentedSection("Description", indentLevel);
        ui.PrintIndentedInfo(this.Description, indentLevel + 1);
    }

    protected virtual void DisplayUsage(ConsoleUI ui, int indentLevel = 0)
    {
        ui.PrintIndentedSection("Usage", indentLevel);
        ui.PrintIndentedInfo(this.Usage, indentLevel + 1);
    }

    protected virtual void DisplaySubcommands(ConsoleUI ui, int indentLevel = 0)
    {
        if (!this.Subcommands.Any())
            return;

        ui.PrintIndentedSection("Subcommands", indentLevel);

        foreach (KeyValuePair<string, string> subcommand in Subcommands)
        {
            ui.PrintIndentedInfo($"{subcommand.Key}: {subcommand.Value}", indentLevel + 1);
        }
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
    protected ArgParser ParseArgs(string[] args)
    {
        ArgParser argParser = new ArgParser(args);

        argParser.Parse();
        return argParser;
    }
}
