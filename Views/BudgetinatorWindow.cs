
using Budgetinator_2000.Controls;
using Budgetinator_2000.Models;
using Budgetinator_2000.Views;

namespace Budgetinator_2000
{
    public partial class BudgetinatorWindow : Form
    {
        private BudgetChart budgetChart;

        private MovablePanel movablePanel;

        private Budget budget = new Budget();
        
        public BudgetinatorWindow()
        {
            InitializeComponent();
            
            // Title of window
            this.Text = "Budgetinator 2000";
            
            // Control where chart is
            budgetChart = new BudgetChart
            {
                Dock = DockStyle.None,
                Size = new Size(1000, 400),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            };
            budgetChart.Location = new Point(
                ClientSize.Width - budgetChart.Width - 10,
                ClientSize.Height - budgetChart.Height - 10
            );

            movablePanel = new MovablePanel();
            Controls.Add(movablePanel);

            Controls.Add(budgetChart);

            // Generate some sample shit
            Load += (s, e) => GenerateSampleData();
        }

        
        private void GenerateSampleData()
        {
            var incomeCategory = new Category { Id = 1, Name = "Income" };
            var housingCategory = new Category { Id = 2, Name = "Housing" };
            var foodCategory = new Category { Id = 3, Name = "Food" };
            var otherCategory = new Category { Id = 4, Name = "Other" };
            
            var random = new Random();
            var transactions = new List<Transaction>();
            
            DateTime today = DateTime.Today;
            
            // One transaction per month
            for (int monthOffset = 0; monthOffset < 12; monthOffset++)
            {
                DateTime monthDate = today.AddMonths(-monthOffset);
                int year = monthDate.Year;
                int month = monthDate.Month;
                
                decimal income = 4000 + (decimal)(random.NextDouble() * 1000);
                transactions.Add(new Transaction
                {
                    Date = new DateTime(year, month, 1),
                    Amount = income,
                    Category = incomeCategory,
                    Description = "Monthly Income",
                    Type = TransactionType.Income
                });
                
                decimal housing = 1000 + (decimal)(random.NextDouble() * 200);
                transactions.Add(new Transaction
                {
                    Date = new DateTime(year, month, 5),
                    Amount = housing,
                    Category = housingCategory,
                    Description = "Rent/Mortgage",
                    Type = TransactionType.Expense
                });
                
                decimal food = 500 + (decimal)(random.NextDouble() * 300);
                transactions.Add(new Transaction
                {
                    Date = new DateTime(year, month, 15),
                    Amount = food,
                    Category = foodCategory,
                    Description = "Groceries",
                    Type = TransactionType.Expense
                });
                
                decimal other = 800 + (decimal)(random.NextDouble() * 700);
                transactions.Add(new Transaction
                {
                    Date = new DateTime(year, month, 20),
                    Amount = other,
                    Category = otherCategory,
                    Description = "Other Expenses",
                    Type = TransactionType.Expense
                });
                
                // Budget Target
                decimal target = 2000 + (decimal)(random.NextDouble() * 1500);
                budget.SetMonthlyTarget(new DateTime(year, month, 1), target);
            }

            DateTime startDate = today.AddMonths(-11);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);
            
            // Update the chart
            budgetChart.SetTransactions(transactions);
            budgetChart.SetBudget(budget);
            budgetChart.SetDateRange(startDate, today);
            budgetChart.Invalidate();
        }
    }
}