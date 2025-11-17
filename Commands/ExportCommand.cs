namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class ExportCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        string? filePath = null;
        int? month = null;

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--file":
                    if (i + 1 < args.Length)
                        filePath = args[++i];
                    break;
                case "--month":
                    if (i + 1 < args.Length && int.TryParse(args[++i], out var m))
                        month = m;
                    break;
            }
        }

        try
        {
            var exportPath = service.ExportToCSV(filePath, month);
            Console.WriteLine($"Expenses exported successfully to: {exportPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Failed to export expenses: {ex.Message}");
        }
    }
}
