namespace ExpenseTracker.Models;

public class Budget
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }

    public override string ToString()
    {
        var monthName = new DateTime(Year, Month, 1).ToString("MMMM");
        return $"{monthName} {Year}: ${Amount:F2}";
    }
}
