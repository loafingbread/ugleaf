namespace PrettyEnough.Utils;

/// <summary>
/// Parses arguments for command inputs to the shell
/// </summary>
public class ArgParser
{
    public Dictionary<string, string> Flags { get; private set; } = new();
    public List<string> PositionalArgs { get; private set; } = new();

    private string[] _args { get; init; }
    private int _i { get; set; }

    public ArgParser(string[] args)
    {
        this._args = args;
        this._i = 0;
    }

    /// <summary>
    /// Check if the command is asking for help. Need to run Parse() first.
    /// </summary>
    /// <returns>
    /// True if command is asking for help regarding the command.
    /// False, if not.
    /// </returns>
    public bool IsHelpRequest()
    {
        if (this.PositionalArgs[0] == "help")
        {
            return true;
        }

        if (this.Flags.ContainsKey("help") || this.Flags.ContainsKey("h"))
        {
            return true;
        }

        return false;
    }

    public bool HasFlag(string longFlag, string shortFlag)
    {
        return this.Flags.ContainsKey(longFlag) || this.Flags.ContainsKey(shortFlag);
    }

    public string GetFlag(string longFlag, string shortFlag)
    {
        if (this.Flags.ContainsKey(longFlag))
        {
            return this.Flags[longFlag];
        }

        if (this.Flags.ContainsKey(shortFlag))
        {
            return this.Flags[shortFlag];
        }

        return "";
    }

    public void Parse()
    {
        for (this._i = 0; this._i < this._args.Length; this._i++)
        {
            string arg = this._args[this._i];
            string? nextArg = this._i + 1 < this._args.Length ? this._args[this._i + 1] : null;

            if (this.parseLongFlag(arg, nextArg))
            {
                continue;
            }
            else if (this.parseShortFlag(arg, nextArg))
            {
                continue;
            }

            this.parsePositionalArguments(arg);
        }
    }

    private bool parseLongFlag(string arg, string? nextArg)
    {
        // Long flag: --flag
        bool isLongFlag = arg.StartsWith("--");
        if (!isLongFlag)
        {
            return false;
        }

        string flagName = arg.Substring(2);

        // If the flag has a value: --flag=value
        bool hasValue = flagName.Contains('=');
        if (hasValue)
        {
            string[] parts = flagName.Split('=', 2);
            this.Flags[parts[0]] = parts[1];
            return true;
        }

        // If the flag has a next argument: --flag value
        if (nextArg != null && !nextArg.StartsWith("-"))
        {
            this.Flags[flagName] = nextArg;
            this.skipNextArg();
            return true;
        }

        // If the flag has no value or next argument, it's a boolean flag
        this.Flags[flagName] = "true";
        return true;
    }

    /// <summary>
    /// Skip parsing next arg by incrementing current arg counter.
    /// Usually only done when parsing resulted in two args being consumed.
    /// E.g. for a key value pair.
    /// </summary>
    private void skipNextArg()
    {
        this._i++;
    }

    private bool parseShortFlag(string arg, string? nextArg)
    {
        // Short flag: -f
        bool isShortFlag = arg.StartsWith("-");
        if (!isShortFlag)
        {
            return false;
        }

        string flagName = arg.Substring(1);

        bool hasValue = flagName.Contains('=');
        if (hasValue)
        {
            string[] parts = flagName.Split('=', 2);
            this.Flags[parts[0]] = parts[1];
            return true;
        }

        if (nextArg != null && !nextArg.StartsWith("-"))
        {
            this.Flags[flagName] = nextArg;
            this.skipNextArg();
            return true;
        }

        this.Flags[flagName] = "true";
        return true;
    }

    private void parsePositionalArguments(string arg)
    {
        this.PositionalArgs.Add(arg);
    }
}