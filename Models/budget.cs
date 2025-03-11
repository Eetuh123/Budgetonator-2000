namespace Budgetinator_2000.Models
{
    public class Budget
    {
        public decimal MonthlyLimit { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public DateTime StartDate { get; set; }

        public Dictionary<string, decimal> MonthlyTargets { get; set; } = new Dictionary<string, decimal>();
        // Here we check if the specific month contains key and if it does we return it
        public decimal GetMonthlyTarget(DateTime date)
        {
            string key = $"{date.Year}-{date.Month:D2}";
            if (MonthlyTargets.ContainsKey(key))
                return MonthlyTargets[key];
            return MonthlyLimit;
        }
        // For setting our target points with Date and Money amount
        public void SetMonthlyTarget(DateTime date, decimal target)
        {
            string key = $"{date.Year}-{date.Month:D2}";
            MonthlyTargets[key] = target;
        }
    }   
}