namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class UpdateCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        int? id = null;
        string? description = null;
        decimal? amount = null;
        string? category = null;

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--id":
                    if (i + 1 < args.Length && int.TryParse(args[++i], out var idVal))
                        id = idVal;
                    break;
                case "--description":
                    if (i + 1 < args.Length)
                        description = args[++i];
                    break;
                case "--amount":
                    if (i + 1 < args.Length && decimal.TryParse(args[++i], out var amt))
                        amount = amt;
                    break;
                case "--category":
                    if (i + 1 < args.Length)
                        category = args[++i];
                    break;
            }
        }

        if (!id.HasValue)
        {
            Console.WriteLine("Error: --id is required");
            return;
        }

        if (string.IsNullOrWhiteSpace(description) && !amount.HasValue && string.IsNullOrWhiteSpace(category))
        {
            Console.WriteLine("Error: At least one of --description, --amount, or --category must be provided");
            return;
        }

        service.UpdateExpense(id.Value, description, amount, category);
        Console.WriteLine("Expense updated successfully");
    }
}
