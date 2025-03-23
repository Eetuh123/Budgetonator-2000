using System;
using System.Collections.Generic;

namespace Budgetinator_2000.Models
{
    public class TransactionService
    {
        //Luo listan joka koostuu transaction Modelissta jota ei voi korvata sen j‰lkeen kun se on luotu
        private readonly List<Transaction> _transactions = new();
        // Functio jota voi k‰yytt‰‰ kaikkialla joka lis‰‰ listaan uuden transactio modellin
        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }
        // 
        public List<Transaction> GetTransactions()
        {
            return _transactions;
        }
    }
}