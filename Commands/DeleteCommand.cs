namespace ExpenseTracker.Commands;

using ExpenseTracker.Services;

public static class DeleteCommand
{
    public static void Execute(ExpenseService service, string[] args)
    {
        int? id = null;

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "--id" && i + 1 < args.Length && int.TryParse(args[++i], out var idVal))
                id = idVal;
        }

        if (!id.HasValue)
        {
            Console.WriteLine("Error: --id is required");
            return;
        }

        service.DeleteExpense(id.Value);
        Console.WriteLine("Expense deleted successfully");
    }
}
