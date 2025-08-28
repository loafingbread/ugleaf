namespace PrettyEnough.UI;

public class ConsoleUI
{
    private readonly ConsoleColor defaultColor;
    private readonly ConsoleColor successColor = ConsoleColor.Green;
    private readonly ConsoleColor errorColor = ConsoleColor.Red;
    private readonly ConsoleColor infoColor = ConsoleColor.Cyan;
    private readonly ConsoleColor warningColor = ConsoleColor.Yellow;
    private readonly ConsoleColor sectionColor = ConsoleColor.Magenta;

    public ConsoleUI()
    {
        defaultColor = Console.ForegroundColor;
    }

    public void PrintWelcome()
    {
        Console.Clear();
        PrintSection("🎮 PrettyEnough - Game Logic Testing Shell");
        PrintInfo("A beautiful testing environment for your game logic");
        PrintInfo("Type 'help' to see available commands");
        PrintInfo("");
    }

    public void PrintPrompt()
    {
        SetColor(infoColor);
        Console.Write("🎯 > ");
        ResetColor();
    }

    public void PrintSuccess(string message)
    {
        SetColor(successColor);
        Console.WriteLine($"✅ {message}");
        ResetColor();
    }

    public void PrintError(string message)
    {
        SetColor(errorColor);
        Console.WriteLine($"❌ {message}");
        ResetColor();
    }

    public void PrintInfo(string message)
    {
        SetColor(infoColor);
        Console.WriteLine($"ℹ️  {message}");
        ResetColor();
    }

    public void PrintWarning(string message)
    {
        SetColor(warningColor);
        Console.WriteLine($"⚠️  {message}");
        ResetColor();
    }

    public void PrintSection(string title)
    {
        SetColor(sectionColor);
        Console.WriteLine($"\n{title}");
        Console.WriteLine(new string('=', title.Length));
        ResetColor();
    }

    public void PrintTable(string[] headers, string[][] rows)
    {
        if (rows.Length == 0) return;

        // Calculate column widths
        var columnWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = headers[i].Length;
            foreach (var row in rows)
            {
                if (i < row.Length && row[i].Length > columnWidths[i])
                    columnWidths[i] = row[i].Length;
            }
        }

        // Print header
        SetColor(sectionColor);
        PrintTableRow(headers, columnWidths);
        Console.WriteLine(new string('-', columnWidths.Sum() + headers.Length * 3 + 1));
        ResetColor();

        // Print rows
        foreach (var row in rows)
        {
            PrintTableRow(row, columnWidths);
        }
        Console.WriteLine();
    }

    private void PrintTableRow(string[] cells, int[] columnWidths)
    {
        Console.Write("|");
        for (int i = 0; i < cells.Length; i++)
        {
            var cell = i < cells.Length ? cells[i] : "";
            Console.Write($" {cell.PadRight(columnWidths[i])} |");
        }
        Console.WriteLine();
    }

    private void SetColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }

    private void ResetColor()
    {
        Console.ForegroundColor = defaultColor;
    }
}
