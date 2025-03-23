using System;
using System.Collections.Generic;

namespace Budgetinator_2000.Models
{
    public class TransactionService
    {
        private readonly List<Transaction> _transactions = new();

        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public List<Transaction> GetTransactions()
        {
            return _transactions;
        }
    }
}