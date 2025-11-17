namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class SummaryCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        int? month = null;
        string? category = null;

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--month":
                    if (i + 1 < args.Length && int.TryParse(args[++i], out var m))
                        month = m;
                    break;
                case "--category":
                    if (i + 1 < args.Length)
                        category = args[++i];
                    break;
            }
        }

        if (month.HasValue && category != null)
        {
            var expenses = service.GetExpensesByMonth(month.Value)
                .Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var total = expenses.Sum(e => e.Amount);
            var monthName = new DateTime(DateTime.Now.Year, month.Value, 1).ToString("MMMM");
            Console.WriteLine($"Total expenses for {monthName} (Category: {category}): ${total:F2}");
            CheckBudget(service, month.Value);
        }
        else if (month.HasValue)
        {
            var total = service.GetTotalExpensesByMonth(month.Value);
            var monthName = new DateTime(DateTime.Now.Year, month.Value, 1).ToString("MMMM");
            Console.WriteLine($"Total expenses for {monthName}: ${total:F2}");
            CheckBudget(service, month.Value);
        }
        else if (!string.IsNullOrWhiteSpace(category))
        {
            var total = service.GetTotalExpensesByCategory(category);
            Console.WriteLine($"Total expenses for {category}: ${total:F2}");
        }
        else
        {
            var total = service.GetTotalExpenses();
            Console.WriteLine($"Total expenses: ${total:F2}");
        }
    }

    private static void CheckBudget(ExpenseService service, int month)
    {
        var budget = service.GetBudget(month);
        if (budget != null)
        {
            var total = service.GetTotalExpensesByMonth(month);
            if (total > budget.Amount)
            {
                var exceeded = total - budget.Amount;
                Console.WriteLine($"⚠️  Warning: Budget exceeded by ${exceeded:F2}!");
            }
            else
            {
                var remaining = budget.Amount - total;
                Console.WriteLine($"✓ Budget remaining: ${remaining:F2}");
            }
        }
    }
}
