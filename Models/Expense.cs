namespace ExpenseTracker.Models;

public class Expense
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public string Category { get; set; } = "";

    public Expense()
    {
        Category = "General";
    }

    public override string ToString()
    {
        return $"{Id,-5} {Date:yyyy-MM-dd} {Description,-25} ${Amount:F2,-10} {Category}";
    }
}
