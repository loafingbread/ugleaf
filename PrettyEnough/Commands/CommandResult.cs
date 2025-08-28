namespace PrettyEnough.Commands;

public class CommandResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public object? Data { get; set; }

    public static CommandResult Ok(string message = "", object? data = null)
    {
        return new CommandResult
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static CommandResult Error(string message)
    {
        return new CommandResult
        {
            Success = false,
            Message = message
        };
    }
}
