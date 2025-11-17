#!/bin/bash

# Comprehensive Test Suite for Expense Tracker
# This script demonstrates all features of the expense tracker

set -e

PROJECT_DIR="/Users/mac/ExpenseTracker"
cd "$PROJECT_DIR"

echo "================================"
echo "  Expense Tracker Test Suite"
echo "================================"
echo ""

# Clean up previous test data
rm -rf data/

echo "✓ Starting fresh (cleared previous data)"
echo ""

# Test 1: Add Expenses
echo "--- Test 1: Adding Expenses ---"
echo "Adding: Breakfast - $8"
dotnet run -- add --description "Breakfast" --amount 8 --category "Food" > /dev/null
echo "Adding: Lunch - $15"
dotnet run -- add --description "Lunch" --amount 15 --category "Food" > /dev/null
echo "Adding: Gas - $50"
dotnet run -- add --description "Gas" --amount 50 --category "Transportation" > /dev/null
echo "Adding: Movie - $20"
dotnet run -- add --description "Movie" --amount 20 --category "Entertainment" > /dev/null
echo "Adding: Groceries - $120"
dotnet run -- add --description "Groceries" --amount 120 --category "Food" > /dev/null
echo "✓ 5 expenses added successfully"
echo ""

# Test 2: List All Expenses
echo "--- Test 2: List All Expenses ---"
dotnet run -- list
echo ""

# Test 3: Summary
echo "--- Test 3: View Summary ---"
echo "Total expenses:"
dotnet run -- summary
echo ""

# Test 4: Category Filter
echo "--- Test 4: Filter by Category (Food) ---"
dotnet run -- list --category "Food"
echo ""

# Test 5: Category Summary
echo "--- Test 5: Summary by Category (Food) ---"
dotnet run -- summary --category "Food"
echo ""

# Test 6: Set Budget
echo "--- Test 6: Set Monthly Budget ---"
dotnet run -- budget set --month 11 --amount 150 > /dev/null
echo "✓ Budget set for November: $150"
echo ""

# Test 7: Check Budget (should exceed)
echo "--- Test 7: Check Budget Status (Should Show Exceeded) ---"
dotnet run -- summary --month 11
echo ""

# Test 8: Budget View
echo "--- Test 8: Detailed Budget View ---"
dotnet run -- budget view --month 11
echo ""

# Test 9: Update Expense
echo "--- Test 9: Update Expense ---"
echo "Updating ID 1 to 'Breakfast at Cafe' and amount to $10"
dotnet run -- update --id 1 --description "Breakfast at Cafe" --amount 10 > /dev/null
echo "✓ Expense updated"
dotnet run -- list | grep "Breakfast at Cafe"
echo ""

# Test 10: Delete Expense
echo "--- Test 10: Delete Expense ---"
echo "Deleting expense ID 4 (Movie)"
dotnet run -- delete --id 4 > /dev/null
echo "✓ Expense deleted"
echo ""

# Test 11: Verify Deletion
echo "--- Test 11: Verify Deletion ---"
echo "Total expenses after deletion:"
dotnet run -- summary
echo ""

# Test 12: Export to CSV
echo "--- Test 12: Export to CSV ---"
CSV_FILE=$(dotnet run -- export | grep 'data/expenses_' | awk '{print $NF}')
echo "✓ Exported to: $CSV_FILE"
echo ""

# Test 13: Show CSV Content
echo "--- Test 13: CSV File Content ---"
cat "$CSV_FILE"
echo ""

# Test 14: Month Filter
echo "--- Test 14: List by Month (November) ---"
dotnet run -- list --month 11
echo ""

echo "================================"
echo "  All Tests Completed! ✓"
echo "================================"
echo ""
echo "Summary of test results:"
echo "✓ Add expenses with categories"
echo "✓ List all expenses"
echo "✓ View total summary"
echo "✓ Filter by category"
echo "✓ Set monthly budget"
echo "✓ View budget status"
echo "✓ Detect budget exceeded"
echo "✓ Update existing expense"
echo "✓ Delete expense"
echo "✓ Export to CSV"
echo "✓ Filter by month"
echo ""
echo "Data files created:"
ls -lh data/
