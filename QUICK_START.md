# Expense Tracker - Quick Start Guide

## Overview
This is a command-line expense tracker application that helps you manage your personal finances.

## Quick Setup

1. Navigate to the project directory:
   ```bash
   cd /Users/mac/ExpenseTracker
   ```

2. Build the application (first time only):
   ```bash
   dotnet build
   ```

## Quick Examples

### 1. Add Your First Expense
```bash
dotnet run -- add --description "Lunch" --amount 20
```

### 2. View All Expenses
```bash
dotnet run -- list
```

### 3. Get a Summary
```bash
dotnet run -- summary
```

### 4. Set a Monthly Budget
```bash
dotnet run -- budget set --month 11 --amount 500
```

### 5. Check if Budget is Exceeded
```bash
dotnet run -- summary --month 11
```

### 6. Export Expenses to CSV
```bash
dotnet run -- export
```

## Sample Workflow

Here's a complete example of using the expense tracker:

```bash
# Add some expenses
dotnet run -- add --description "Morning Coffee" --amount 5 --category "Food"
dotnet run -- add --description "Groceries" --amount 100 --category "Food"
dotnet run -- add --description "Gas" --amount 50 --category "Transportation"
dotnet run -- add --description "Netflix" --amount 15 --category "Entertainment"

# List all expenses
dotnet run -- list

# Set a budget for this month
dotnet run -- budget set --month 11 --amount 300

# Check your spending against the budget
dotnet run -- summary --month 11

# See how much you spent on Food
dotnet run -- list --category "Food"
dotnet run -- summary --category "Food"

# Update an expense if you made a mistake
dotnet run -- update --id 1 --amount 6

# Delete an expense
dotnet run -- delete --id 4

# Export your expenses
dotnet run -- export
```

## Common Commands Cheat Sheet

| Task | Command |
|------|---------|
| Add expense | `dotnet run -- add --description "X" --amount 10` |
| Add with category | `dotnet run -- add --description "X" --amount 10 --category "Food"` |
| List all | `dotnet run -- list` |
| List by category | `dotnet run -- list --category "Food"` |
| List by month | `dotnet run -- list --month 11` |
| View total | `dotnet run -- summary` |
| Month summary | `dotnet run -- summary --month 11` |
| Category summary | `dotnet run -- summary --category "Food"` |
| Update | `dotnet run -- update --id 1 --description "New"` |
| Delete | `dotnet run -- delete --id 1` |
| Set budget | `dotnet run -- budget set --month 11 --amount 500` |
| View budget | `dotnet run -- budget view --month 11` |
| Export CSV | `dotnet run -- export` |

## Data Files

Your data is stored in JSON format in the `data/` folder:
- `data/expenses.json` - All your expenses
- `data/budgets.json` - Your budget settings
- `data/expenses_*.csv` - Exported files

## Tips

1. **Always use --category** when adding expenses to better organize them
2. **Set monthly budgets** to track your spending against limits
3. **Use summaries** to get quick spending insights
4. **Export regularly** to backup your data
5. **Check budget before month-end** to avoid overspending

## Help

For detailed documentation, see README.md in the project directory.

## Troubleshooting

**Q: "Error: --description is required"**
A: Make sure you use `--description` flag when adding an expense.

**Q: "Error: Amount cannot be negative"**
A: Amounts must be positive numbers.

**Q: "Expense with ID X not found"**
A: Check the expense ID using `dotnet run -- list` before updating/deleting.

**Q: "No budget set for [month]"**
A: Use `dotnet run -- budget set --month X --amount Y` to set a budget first.

## Next Steps

- Read the full README.md for advanced features
- Explore the source code to understand the implementation
- Consider adding custom categories based on your spending habits
- Use CSV export to analyze spending patterns in Excel
