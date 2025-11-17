namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class BudgetCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        string? action = null;
        int? month = null;
        decimal? amount = null;

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "set":
                case "view":
                case "list":
                    action = args[i];
                    break;
                case "--month":
                    if (i + 1 < args.Length && int.TryParse(args[++i], out var m))
                        month = m;
                    break;
                case "--amount":
                    if (i + 1 < args.Length && decimal.TryParse(args[++i], out var amt))
                        amount = amt;
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(action))
        {
            Console.WriteLine("Error: action required (set, view, or list)");
            return;
        }

        switch (action.ToLower())
        {
            case "set":
                HandleSet(service, month, amount);
                break;
            case "view":
                HandleView(service, month);
                break;
            case "list":
                HandleList(service);
                break;
        }
    }

    private static void HandleSet(ExpenseService service, int? month, decimal? amount)
    {
        if (!month.HasValue)
        {
            Console.WriteLine("Error: --month is required");
            return;
        }

        if (!amount.HasValue || amount <= 0)
        {
            Console.WriteLine("Error: --amount is required and must be greater than 0");
            return;
        }

        service.SetBudget(month.Value, amount.Value);
        var monthName = new DateTime(DateTime.Now.Year, month.Value, 1).ToString("MMMM");
        Console.WriteLine($"Budget set for {monthName}: ${amount:F2}");
    }

    private static void HandleView(ExpenseService service, int? month)
    {
        if (!month.HasValue)
        {
            Console.WriteLine("Error: --month is required");
            return;
        }

        var budget = service.GetBudget(month.Value);
        if (budget == null)
        {
            var monthName = new DateTime(DateTime.Now.Year, month.Value, 1).ToString("MMMM");
            Console.WriteLine($"No budget set for {monthName}");
            return;
        }

        var total = service.GetTotalExpensesByMonth(month.Value);
        var monthName2 = new DateTime(DateTime.Now.Year, month.Value, 1).ToString("MMMM");
        Console.WriteLine($"{monthName2} Budget: ${budget.Amount:F2}");
        Console.WriteLine($"Total Expenses: ${total:F2}");

        if (total > budget.Amount)
        {
            var exceeded = total - budget.Amount;
            Console.WriteLine($"Status: ⚠️  EXCEEDED by ${exceeded:F2}");
        }
        else
        {
            var remaining = budget.Amount - total;
            Console.WriteLine($"Status: ✓ Remaining: ${remaining:F2}");
        }
    }

    private static void HandleList(ExpenseService service)
    {
        // For simplicity, just show current year budgets
        Console.WriteLine("Budgets for current year:");
        for (int month = 1; month <= 12; month++)
        {
            var budget = service.GetBudget(month);
            if (budget != null)
            {
                var monthName = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");
                var total = service.GetTotalExpensesByMonth(month);
                var status = total > budget.Amount ? "⚠️  EXCEEDED" : "✓";
                Console.WriteLine($"{monthName,-12} ${budget.Amount:F2,-10} (Spent: ${total:F2}) {status}");
            }
        }
    }
}
