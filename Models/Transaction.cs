namespace Budgetinator_2000.Models
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal? Amount { get; set; }
        public Category? Category { get; set; }
        public string? Description  { get; set; }
        public TransactionType Type { get; set; }
    }
}
