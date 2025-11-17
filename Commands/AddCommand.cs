namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class AddCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        string? description = null;
        decimal amount = 0;
        string? category = null;

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
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

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Error: --description is required");
            return;
        }

        if (amount <= 0)
        {
            Console.WriteLine("Error: --amount is required and must be greater than 0");
            return;
        }

        var expense = service.AddExpense(description, amount, category);
        Console.WriteLine($"Expense added successfully (ID: {expense.Id})");
    }
}
