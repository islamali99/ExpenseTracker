# Expense Tracker - Architecture Overview

## Application Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      Command Line Input                      │
│              dotnet run -- <command> [options]               │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────┐
│                     Program.cs                               │
│        - Parse command line arguments                        │
│        - Route to appropriate command handler                │
└──────┬─────────────┬─────────────┬──────────┬──────┬──────┬─┘
       │             │             │          │      │      │
       ▼             ▼             ▼          ▼      ▼      ▼
   ┌───────┐  ┌──────────┐  ┌───────────┐ ┌──────┐┌──────┐┌────┐
   │ Add   │  │ Update   │  │ Delete    │ │List  ││Summary│Export
   │Command│  │Command   │  │Command    │ │Cmd  ││Cmd    │Cmd
   └───┬───┘  └────┬─────┘  └────┬──────┘ └──┬──┘└───┬───┘└──┬─┘
       │            │            │           │       │       │
       │ (All commands use)      │           │       │       │
       │                         │           │       │       │
       └─────────────────────────┼───────────┴───────┴───────┘
                                 │
                                 ▼
                    ┌────────────────────────┐
                    │  ExpenseService        │
                    │                        │
                    │  Core Business Logic:  │
                    │  - Add/Update/Delete   │
                    │  - Query/Filter        │
                    │  - Budget Management   │
                    │  - Export              │
                    └────────┬───────────────┘
                             │
                    ┌────────┴───────────┐
                    │                    │
                    ▼                    ▼
            ┌──────────────┐    ┌──────────────┐
            │  Expense     │    │  Budget      │
            │  Model       │    │  Model       │
            │              │    │              │
            │  - Id        │    │  - Month     │
            │  - Date      │    │  - Year      │
            │  - Desc      │    │  - Amount    │
            │  - Amount    │    │              │
            │  - Category  │    │              │
            └──────────────┘    └──────────────┘
                    │                    │
                    └────────┬───────────┘
                             │
            ┌────────────────▼──────────────┐
            │      Data Persistence         │
            │                               │
            │  - data/expenses.json         │
            │  - data/budgets.json          │
            │  - data/expenses_*.csv        │
            │                               │
            │  (JSON serialization via      │
            │   System.Text.Json)           │
            └───────────────────────────────┘
```

## Data Flow Diagrams

### Add Expense Flow
```
User Input
    │
    ├─> Parse: --description "Lunch" --amount 20 --category "Food"
    │
    ▼
AddCommand.Execute()
    │
    ├─> Validate inputs (not empty, amount > 0)
    │
    ├─> Call ExpenseService.AddExpense()
    │   │
    │   ├─> Create new Expense object with:
    │   │   - Id (auto-increment)
    │   │   - Date (current date/time)
    │   │   - Description
    │   │   - Amount
    │   │   - Category
    │   │
    │   ├─> Add to in-memory list
    │   │
    │   └─> Save to data/expenses.json
    │
    ▼
Display: "Expense added successfully (ID: 1)"
```

### List & Filter Flow
```
User Input: list --category "Food" --month 11
    │
    ▼
ListCommand.Execute()
    │
    ├─> Get all expenses from ExpenseService
    │
    ├─> Apply filters:
    │   ├─> Category filter (if provided)
    │   └─> Month filter (if provided)
    │
    ├─> Format data in table
    │
    ▼
Display formatted table with ID, Date, Description, Amount, Category
```

### Budget Management Flow
```
User: budget set --month 11 --amount 500
    │
    ▼
BudgetCommand.Execute()
    │
    ├─> Parse month and amount
    │
    ├─> Call ExpenseService.SetBudget()
    │   │
    │   ├─> Check if budget exists for month/year
    │   │
    │   ├─> Create or update Budget object
    │   │
    │   └─> Save to data/budgets.json
    │
    ▼
Display: "Budget set for November: $500"

---

User: summary --month 11
    │
    ▼
SummaryCommand.Execute()
    │
    ├─> Get total expenses for November
    │
    ├─> Get budget for November
    │
    ├─> Compare expenses vs budget
    │   │
    │   ├─> If expenses > budget:
    │   │   └─> Display "⚠️ Warning: Budget exceeded by $X"
    │   │
    │   └─> If expenses <= budget:
    │       └─> Display "✓ Budget remaining: $X"
    │
    ▼
Display summary with budget status
```

## Class Relationships

```
┌─────────────────────────┐
│    Program.cs           │
│  (Main Entry Point)     │
└────────────┬────────────┘
             │ uses
             ▼
┌─────────────────────────────────────────────────────────────┐
│              ExpenseService                                  │
│  ─────────────────────────────────────────────────────────  │
│  - List<Expense> expenses                                   │
│  - List<Budget> budgets                                     │
│  - int nextId                                               │
│  ─────────────────────────────────────────────────────────  │
│  + AddExpense()                                             │
│  + UpdateExpense()                                          │
│  + DeleteExpense()                                          │
│  + GetAllExpenses()                                         │
│  + GetExpensesByMonth()                                     │
│  + GetExpensesByCategory()                                  │
│  + GetTotalExpenses()                                       │
│  + SetBudget()                                              │
│  + GetBudget()                                              │
│  + IsBudgetExceeded()                                       │
│  + ExportToCSV()                                            │
│  - LoadData()                                               │
│  - SaveExpenses()                                           │
│  - SaveBudgets()                                            │
└──────┬──────────────────────────────────────────────────────┘
       │ contains                               │ contains
       ▼                                         ▼
┌─────────────────────────┐          ┌────────────────────────┐
│    Expense              │          │     Budget             │
│  ─────────────────────  │          │  ───────────────────   │
│  + Id: int              │          │  + Month: int          │
│  + Date: DateTime       │          │  + Year: int           │
│  + Description: string  │          │  + Amount: decimal     │
│  + Amount: decimal      │          │                        │
│  + Category: string     │          │  + ToString()          │
│                         │          │                        │
│  + ToString()           │          └────────────────────────┘
└─────────────────────────┘

         │ used by
         ▼
┌──────────────────────────────────────────────────────────────┐
│                      Commands                                │
│  ──────────────────────────────────────────────────────────  │
│  • AddCommand.Execute()                                      │
│  • UpdateCommand.Execute()                                   │
│  • DeleteCommand.Execute()                                   │
│  • ListCommand.Execute()                                     │
│  • SummaryCommand.Execute()                                  │
│  • BudgetCommand.Execute()                                   │
│  • ExportCommand.Execute()                                   │
└──────────────────────────────────────────────────────────────┘
```

## File I/O Operations

```
Application Startup
    │
    ├─> Check if data/ directory exists
    │   └─> If not, create it
    │
    ├─> Load data/expenses.json
    │   └─> Parse JSON → List<Expense>
    │
    └─> Load data/budgets.json
        └─> Parse JSON → List<Budget>

---

On Any Data Modification (Add/Update/Delete)
    │
    ├─> Update in-memory list
    │
    └─> Serialize to JSON
        └─> Save to file

---

On Export Command
    │
    └─> Create CSV file
        └─> Save to data/expenses_YYYYMMDD_HHMMSS.csv
```

## Error Handling Strategy

```
User Input
    │
    ▼
┌─────────────────────────────────┐
│  Command Validation             │
│  - Check required arguments     │
│  - Parse numeric values         │
│  - Validate ranges (months)     │
└────────┬────────────────────────┘
         │
    ┌────┴────┐
    │          │
    ▼          ▼
 Valid      Invalid
    │          │
    │          └──> Show error message
    │
    ▼
┌─────────────────────────────────┐
│  Service Operation              │
│  - Validate business logic      │
│  - Check constraints            │
│  - Verify data integrity        │
└────────┬────────────────────────┘
         │
    ┌────┴────┐
    │          │
    ▼          ▼
Success    Failure
    │          │
    │          └──> Throw exception
    │
    ▼
┌─────────────────────────────────┐
│  Success Message                │
│  Display results/confirmation   │
└─────────────────────────────────┘
```

## Technology Stack

```
┌─────────────────────────────────────┐
│        Expense Tracker CLI          │
├─────────────────────────────────────┤
│ Language:     C# 9.0                │
│ Runtime:      .NET 9.0              │
│ UI:           Console (CLI)         │
│ Data Format:  JSON                  │
│ Serializer:   System.Text.Json      │
│ Architecture: Command Pattern       │
│ Storage:      File-based            │
└─────────────────────────────────────┘
```

## Deployment Model

```
Developer's Machine
        │
        ├─> Source Code (C#)
        │
        ├─> dotnet build
        │
        ├─> Compiled Binary (dll)
        │
        └─> Runtime Execution
            │
            ├─> data/ (Created at first run)
            │   ├─> expenses.json
            │   ├─> budgets.json
            │   └─> expenses_*.csv
            │
            └─> Command Line Interface
```

## State Management

```
ExpenseService (Singleton-like)
    │
    ├─> In-Memory State
    │   ├─> List<Expense>
    │   ├─> List<Budget>
    │   └─> int nextId
    │
    ├─> Data Persistence
    │   ├─> Load on initialization
    │   ├─> Save on modifications
    │   └─> File-based storage
    │
    └─> Query Operations
        ├─> Stateless queries
        ├─> Filtering & aggregation
        └─> No external state changes
```

---

This architecture provides:
✅ Clean separation of concerns
✅ Easy to test and maintain
✅ Scalable for future features
✅ Simple data persistence
✅ Robust error handling
✅ User-friendly CLI interface
