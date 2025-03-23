using System;
using System.Collections.Generic;
using System.Linq;
using Budgetinator_2000.Models;

namespace Budgetinator_2000.Mathcalculations
{
    public static class BudgetCalculations
    {
        private static decimal CalculateAnnualTotal(IEnumerable<decimal> monthlyValues, string collectionName)
        {
            if (monthlyValues == null)
                throw new ArgumentException($"{collectionName} cannot be null.");

            if (!monthlyValues.Any())
                throw new ArgumentException($"{collectionName} cannot be empty.");

            decimal sum = 0m;
            foreach (var value in monthlyValues)
            {
                sum += value;
            }
            return sum;
        }

        public static decimal CalculateAnnualSavings(IEnumerable<decimal> monthlySavings)
        {
            return CalculateAnnualTotal(monthlySavings, "Monthly savings collection");
        }

        public static decimal CalculateAnnualExpenses(IEnumerable<decimal> monthlyExpenses)
        {
            return CalculateAnnualTotal(monthlyExpenses, "Monthly expenses collection");
        }

        private static decimal CalculateMonthlyTotal(IEnumerable<Transaction> transactions, TransactionType type, int year, int month)
        {
            if (transactions == null)
                throw new ArgumentException("Transaction list cannot be null.");

            decimal sum = 0m;
            foreach (var transaction in transactions)
            {
                if (transaction.Type == type &&
                    transaction.Date.Year == year &&
                    transaction.Date.Month == month)
                {
                    sum += transaction.Amount ?? 0m;
                }
            }
            return sum;
        }

        public static decimal CalculateMonthlyIncome(IEnumerable<Transaction> transactions, int year, int month)
        {
            return CalculateMonthlyTotal(transactions, TransactionType.Income, year, month);
        }

        public static decimal CalculateMonthlyExpenses(IEnumerable<Transaction> transactions, int year, int month)
        {
            return CalculateMonthlyTotal(transactions, TransactionType.Expense, year, month);
        }

        // Monthly savings calculation
        public static decimal CalculateMonthlySavings(decimal income, decimal expenses)
        {
            return income - expenses;
        }

        // Other useful budget calculations
        public static decimal CalculateExpensePercentage(decimal expenses, decimal income)
        {
            if (income == 0)
                throw new DivideByZeroException("Income cannot be zero.");

            return (expenses / income) * 100;
        }

        public static decimal CalculateDebtRatio(decimal totalDebts, decimal totalAssets)
        {
            if (totalAssets == 0)
                throw new DivideByZeroException("Total assets cannot be zero.");

            return (totalDebts / totalAssets) * 100;
        }
    }
}
