using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Commands;

var expenseService = new ExpenseService();

if (args.Length == 0)
{
    Console.WriteLine("Expense Tracker CLI");
    Console.WriteLine("Usage: expense-tracker <command> [options]");
    Console.WriteLine("\nAvailable commands:");
    Console.WriteLine("  add              Add a new expense");
    Console.WriteLine("  update           Update an existing expense");
    Console.WriteLine("  delete           Delete an expense");
    Console.WriteLine("  list             List all expenses");
    Console.WriteLine("  summary          View summary of all expenses");
    Console.WriteLine("  budget           Manage monthly budget");
    Console.WriteLine("  export           Export expenses to CSV");
    return;
}

var command = args[0].ToLower();

try
{
    switch (command)
    {
        case "add":
            AddCommand.Execute(expenseService, args);
            break;
        case "update":
            UpdateCommand.Execute(expenseService, args);
            break;
        case "delete":
            DeleteCommand.Execute(expenseService, args);
            break;
        case "list":
            ListCommand.Execute(expenseService, args);
            break;
        case "summary":
            SummaryCommand.Execute(expenseService, args);
            break;
        case "budget":
            BudgetCommand.Execute(expenseService, args);
            break;
        case "export":
            ExportCommand.Execute(expenseService, args);
            break;
        default:
            Console.WriteLine($"Unknown command: {command}");
            Console.WriteLine("Run 'expense-tracker' for help.");
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
