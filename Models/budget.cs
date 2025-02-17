namespace Budgetinator_2000.Models
{
    public class Budget
    {
        public decimal MonthlyLimit { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public DateTime StartDate { get; set; }
    }   
}