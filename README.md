# Expense Tracker CLI

A comprehensive command-line expense tracker application built with C# (.NET 9). Manage your finances by adding, updating, deleting, and analyzing expenses with built-in budget tracking and CSV export capabilities.

## Features

### Core Features
- ✅ **Add Expenses**: Add new expenses with description, amount, and optional category
- ✅ **Update Expenses**: Modify existing expenses (description, amount, or category)
- ✅ **Delete Expenses**: Remove expenses by ID
- ✅ **List Expenses**: View all expenses with optional filtering by category or month
- ✅ **Summary**: Get a summary of all expenses or filtered by month/category
- ✅ **Categories**: Organize expenses by category (default: "General")

### Advanced Features
- 💰 **Monthly Budget**: Set and manage budgets for each month
- ⚠️ **Budget Warnings**: Automatic alerts when expenses exceed the budget
- 📊 **CSV Export**: Export expenses to CSV format for analysis
- 💾 **Data Persistence**: All data is saved to JSON files in the `data/` directory

## Installation

### Prerequisites
- .NET 9.0 or higher
- macOS, Linux, or Windows

### Build from Source
```bash
cd ExpenseTracker
dotnet build
```

## Usage

### Basic Commands

#### Add an Expense
```bash
dotnet run -- add --description "Lunch" --amount 20
dotnet run -- add --description "Groceries" --amount 50 --category "Food"
```

#### List All Expenses
```bash
dotnet run -- list
```

Output:
```
ID    Date       Description              Amount      Category
----  ----------  -------------------------  ----------  --------
1     2025-11-17 Lunch                     $    20,00  General
2     2025-11-17 Groceries                 $    50,00  Food
```

#### View Summary
```bash
# All expenses
dotnet run -- summary

# Expenses for a specific month
dotnet run -- summary --month 11

# Expenses for a specific category
dotnet run -- summary --category "Food"
```

#### Update an Expense
```bash
dotnet run -- update --id 1 --description "Lunch at restaurant"
dotnet run -- update --id 1 --amount 25 --category "Dining"
```

#### Delete an Expense
```bash
dotnet run -- delete --id 1
```

### Budget Management

#### Set a Monthly Budget
```bash
dotnet run -- budget set --month 11 --amount 500
```

#### View Budget Status
```bash
# View budget for a specific month
dotnet run -- budget view --month 11

# List all budgets for the year
dotnet run -- budget list
```

### Export & Analytics

#### Export to CSV
```bash
# Export all expenses
dotnet run -- export

# Export expenses for a specific month
dotnet run -- export --month 11

# Export to a specific file
dotnet run -- export --file my_expenses.csv
```

## Command Reference

### add
Add a new expense
```bash
dotnet run -- add --description <text> --amount <number> [--category <text>]
```

**Options:**
- `--description` (required): Description of the expense
- `--amount` (required): Amount in dollars
- `--category` (optional): Category name (default: "General")

### update
Update an existing expense
```bash
dotnet run -- update --id <number> [--description <text>] [--amount <number>] [--category <text>]
```

**Options:**
- `--id` (required): ID of the expense to update
- `--description` (optional): New description
- `--amount` (optional): New amount
- `--category` (optional): New category

### delete
Delete an expense
```bash
dotnet run -- delete --id <number>
```

**Options:**
- `--id` (required): ID of the expense to delete

### list
List expenses
```bash
dotnet run -- list [--category <text>] [--month <number>]
```

**Options:**
- `--category` (optional): Filter by category
- `--month` (optional): Filter by month (1-12)

### summary
View expense summary
```bash
dotnet run -- summary [--month <number>] [--category <text>]
```

**Options:**
- `--month` (optional): Month number (1-12)
- `--category` (optional): Filter by category

### budget
Manage monthly budgets
```bash
dotnet run -- budget set --month <number> --amount <number>
dotnet run -- budget view --month <number>
dotnet run -- budget list
```

**Subcommands:**
- `set`: Set a budget for a month
- `view`: View budget and spending for a month
- `list`: List all budgets for the year

### export
Export expenses to CSV
```bash
dotnet run -- export [--file <path>] [--month <number>]
```

**Options:**
- `--file` (optional): Output file path
- `--month` (optional): Export only expenses from a specific month

## Data Storage

All data is stored in JSON format in the `data/` directory:
- `data/expenses.json` - All expense records
- `data/budgets.json` - Monthly budget settings
- `data/expenses_YYYYMMDD_HHMMSS.csv` - Exported CSV files

## Examples

### Complete Workflow
```bash
# Add some expenses
dotnet run -- add --description "Lunch" --amount 20 --category "Food"
dotnet run -- add --description "Gas" --amount 50 --category "Transportation"
dotnet run -- add --description "Movie" --amount 15 --category "Entertainment"

# View all expenses
dotnet run -- list

# Set a budget
dotnet run -- budget set --month 11 --amount 200

# Check summary
dotnet run -- summary --month 11

# Filter by category
dotnet run -- list --category "Food"

# Export expenses
dotnet run -- export

# Update an expense
dotnet run -- update --id 1 --amount 25

# Delete an expense
dotnet run -- delete --id 3
```

## Error Handling

The application includes robust error handling for:
- Invalid input values (negative amounts, non-existent IDs)
- Missing required arguments
- File I/O errors
- Invalid month numbers

## Project Structure

```
ExpenseTracker/
├── Program.cs                    # Main entry point
├── Models/
│   ├── Expense.cs               # Expense model
│   └── Budget.cs                # Budget model
├── Services/
│   └── ExpenseService.cs         # Core business logic
├── Commands/
│   ├── AddCommand.cs             # Add command handler
│   ├── UpdateCommand.cs          # Update command handler
│   ├── DeleteCommand.cs          # Delete command handler
│   ├── ListCommand.cs            # List command handler
│   ├── SummaryCommand.cs         # Summary command handler
│   ├── BudgetCommand.cs          # Budget command handler
│   └── ExportCommand.cs          # Export command handler
├── data/                         # Data storage directory (auto-created)
├── ExpenseTracker.csproj         # Project file
└── README.md                     # This file
```

## Future Enhancements

- Recurring expenses
- Expense statistics and charts
- Multiple users/accounts
- Cloud sync support
- Web UI interface
- Mobile app support

## License

MIT License - feel free to use this for your personal finance management.

## Contributing

Contributions are welcome! Feel free to submit issues and pull requests.
