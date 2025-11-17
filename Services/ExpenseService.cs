namespace ExpenseTracker.Services;

using ExpenseTracker.Models;
using System.Text.Json;

public class ExpenseService
{
    private const string DataDir = "data";
    private const string ExpensesFile = "expenses.json";
    private const string BudgetsFile = "budgets.json";
    private List<Expense> expenses;
    private List<Budget> budgets;
    private int nextId;

    public ExpenseService()
    {
        expenses = new List<Expense>();
        budgets = new List<Budget>();
        nextId = 1;
        LoadData();
    }

    private void EnsureDataDirectory()
    {
        if (!Directory.Exists(DataDir))
        {
            Directory.CreateDirectory(DataDir);
        }
    }

    private void LoadData()
    {
        EnsureDataDirectory();
        LoadExpenses();
        LoadBudgets();
    }

    private void LoadExpenses()
    {
        var path = Path.Combine(DataDir, ExpensesFile);
        if (File.Exists(path))
        {
            try
            {
                var json = File.ReadAllText(path);
                expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
                if (expenses.Any())
                {
                    nextId = expenses.Max(e => e.Id) + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not load expenses: {ex.Message}");
                expenses = new List<Expense>();
            }
        }
    }

    private void LoadBudgets()
    {
        var path = Path.Combine(DataDir, BudgetsFile);
        if (File.Exists(path))
        {
            try
            {
                var json = File.ReadAllText(path);
                budgets = JsonSerializer.Deserialize<List<Budget>>(json) ?? new List<Budget>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not load budgets: {ex.Message}");
                budgets = new List<Budget>();
            }
        }
    }

    private void SaveExpenses()
    {
        EnsureDataDirectory();
        var path = Path.Combine(DataDir, ExpensesFile);
        var json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    private void SaveBudgets()
    {
        EnsureDataDirectory();
        var path = Path.Combine(DataDir, BudgetsFile);
        var json = JsonSerializer.Serialize(budgets, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public Expense AddExpense(string description, decimal amount, string? category = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty");
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative");

        var expense = new Expense
        {
            Id = nextId++,
            Date = DateTime.Now,
            Description = description,
            Amount = amount,
            Category = category ?? "General"
        };

        expenses.Add(expense);
        SaveExpenses();
        return expense;
    }

    public void UpdateExpense(int id, string? description = null, decimal? amount = null, string? category = null)
    {
        var expense = expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null)
            throw new InvalidOperationException($"Expense with ID {id} not found");

        if (!string.IsNullOrWhiteSpace(description))
            expense.Description = description;
        if (amount.HasValue)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative");
            expense.Amount = amount.Value;
        }
        if (!string.IsNullOrWhiteSpace(category))
            expense.Category = category;

        SaveExpenses();
    }

    public void DeleteExpense(int id)
    {
        var expense = expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null)
            throw new InvalidOperationException($"Expense with ID {id} not found");

        expenses.Remove(expense);
        SaveExpenses();
    }

    public List<Expense> GetAllExpenses()
    {
        return expenses.OrderBy(e => e.Date).ToList();
    }

    public List<Expense> GetExpensesByMonth(int month, int year = 0)
    {
        if (year == 0)
            year = DateTime.Now.Year;

        return expenses
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .OrderBy(e => e.Date)
            .ToList();
    }

    public List<Expense> GetExpensesByCategory(string category)
    {
        return expenses
            .Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.Date)
            .ToList();
    }

    public decimal GetTotalExpenses()
    {
        return expenses.Sum(e => e.Amount);
    }

    public decimal GetTotalExpensesByMonth(int month, int year = 0)
    {
        if (year == 0)
            year = DateTime.Now.Year;

        return expenses
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .Sum(e => e.Amount);
    }

    public decimal GetTotalExpensesByCategory(string category)
    {
        return expenses
            .Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .Sum(e => e.Amount);
    }

    public List<string> GetAllCategories()
    {
        return expenses.Select(e => e.Category).Distinct().OrderBy(c => c).ToList();
    }

    public void SetBudget(int month, decimal amount, int year = 0)
    {
        if (year == 0)
            year = DateTime.Now.Year;
        if (amount < 0)
            throw new ArgumentException("Budget amount cannot be negative");

        var existing = budgets.FirstOrDefault(b => b.Month == month && b.Year == year);
        if (existing != null)
        {
            existing.Amount = amount;
        }
        else
        {
            budgets.Add(new Budget { Month = month, Year = year, Amount = amount });
        }

        SaveBudgets();
    }

    public Budget? GetBudget(int month, int year = 0)
    {
        if (year == 0)
            year = DateTime.Now.Year;

        return budgets.FirstOrDefault(b => b.Month == month && b.Year == year);
    }

    public bool IsBudgetExceeded(int month, int year = 0)
    {
        if (year == 0)
            year = DateTime.Now.Year;

        var budget = GetBudget(month, year);
        if (budget == null)
            return false;

        var total = GetTotalExpensesByMonth(month, year);
        return total > budget.Amount;
    }

    public string ExportToCSV(string? filePath = null, int? month = null, int? year = null)
    {
        filePath ??= Path.Combine(DataDir, $"expenses_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        EnsureDataDirectory();

        var expensesToExport = expenses;
        if (month.HasValue)
        {
            var y = year ?? DateTime.Now.Year;
            expensesToExport = GetExpensesByMonth(month.Value, y);
        }

        var lines = new List<string> { "ID,Date,Description,Amount,Category" };
        lines.AddRange(expensesToExport.Select(e => 
            $"{e.Id},{e.Date:yyyy-MM-dd},\"{e.Description}\",{e.Amount:F2},{e.Category}"));

        File.WriteAllLines(filePath, lines);
        return filePath;
    }
}
