using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgetinator_2000.Models
{
    public static class TransactionFilter
    {
        public static List<Transaction> FilterTransactions(
            List<Transaction> transactions,
            string? searchTerm = null,
            Category? category = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TransactionType? type = null)
        {
            var filteredTransactions = transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredTransactions = filteredTransactions.Where(t =>
                    (t.Description != null && t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Category != null && t.Category.Name != null && t.Category.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (category != null)
            {
                filteredTransactions = filteredTransactions.Where(t => t.Category != null && t.Category.Id == category.Id);
            }

            if (startDate.HasValue)
            {
                filteredTransactions = filteredTransactions.Where(t => t.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                filteredTransactions = filteredTransactions.Where(t => t.Date <= endDate.Value);
            }

            if (type.HasValue)
            {
                filteredTransactions = filteredTransactions.Where(t => t.Type == type.Value);
            }

            return filteredTransactions.ToList();
        }
    }
}
