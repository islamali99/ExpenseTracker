namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class ListCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        string? category = null;
        int? month = null;

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--category":
                    if (i + 1 < args.Length)
                        category = args[++i];
                    break;
                case "--month":
                    if (i + 1 < args.Length && int.TryParse(args[++i], out var m))
                        month = m;
                    break;
            }
        }

        var expenses = service.GetAllExpenses();

        if (month.HasValue)
        {
            expenses = service.GetExpensesByMonth(month.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            expenses = expenses.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (expenses.Count == 0)
        {
            Console.WriteLine("No expenses found");
            return;
        }

        Console.WriteLine("ID    Date       Description              Amount      Category");
        Console.WriteLine("----  ----------  -------------------------  ----------  --------");
        foreach (var expense in expenses)
        {
            Console.WriteLine($"{expense.Id,-4}  {expense.Date:yyyy-MM-dd} {expense.Description,-25} ${expense.Amount,9:F2}  {expense.Category}");
        }
    }
}
