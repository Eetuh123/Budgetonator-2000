using Budgetinator_2000.Controls;
using Budgetinator_2000.Models;
using Budgetinator_2000.Views;

namespace Budgetinator_2000
{
    public partial class BudgetinatorWindow : Form
    {
        private BudgetChart budgetChart;

        private TransactionHistory transactionHistory;
        private TransactionService transactionService = new TransactionService();

        private MovablePanel movablePanel;

        private Budget budget = new Budget();

        private Button openMovablePanelButton;

        public BudgetinatorWindow()
        {
            InitializeComponent();
            this.Text = "Budgetinator 2000";

            // Budget Chart
            budgetChart = new BudgetChart
            {
                Dock = DockStyle.None,
                Size = new Size(1000, 400),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(
                    ClientSize.Width - 1000 - 10,
                    ClientSize.Height - 400 - 10
                )
            };

            // Scroll thing for history
            Panel scrollPanel = new Panel
            {
                AutoScroll = true,
                Size = new Size(400, ClientSize.Height),
                Location = new Point(10, 10)
            };

            // Search Box bro
            TextBox searchBox = new TextBox
            {
                Location = new Point(0, 0),
                Width = scrollPanel.Width - SystemInformation.VerticalScrollBarWidth
            };

            // Create History
            transactionHistory = new TransactionHistory
            {
                AutoSize = false,
                Width = scrollPanel.Width - SystemInformation.VerticalScrollBarWidth,
                Location = new Point(0, searchBox.Height + 5),
                Dock = DockStyle.Top,
            };

            // Search Enter Key = Working
            searchBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    string searchTerm = searchBox.Text;
                    var fullList = transactionService.GetTransactions();
                    var filtered = TransactionFilter.FilterTransactions(fullList, searchTerm);
                    transactionHistory.SetTransactions(filtered);
                    transactionHistory.Invalidate();
                }
            };

            // Reziseer
            Resize += (s, e) =>
            {
                scrollPanel.Height = ClientSize.Height - 20;
                searchBox.Width = scrollPanel.Width - SystemInformation.VerticalScrollBarWidth;
            };
            // Add controls to form
            scrollPanel.Controls.Add(searchBox);
            scrollPanel.Controls.Add(transactionHistory);
            Controls.Add(scrollPanel);
            Controls.Add(budgetChart);

            // Button to open the movable panel
            openMovablePanelButton = new Button
            {
                Text = "Add transaction",
                Size = new Size(120, 40),
                Location = new Point(600, 600),
                BackColor = Color.LightGray
            };
            openMovablePanelButton.Click += (s, e) =>
            {
                if (movablePanel == null || movablePanel.IsDisposed)
                {
                    movablePanel = new MovablePanel(transactionService);
                    movablePanel.SetTransactionHistory(transactionHistory);
                    movablePanel.SetBudgetChart(budgetChart);
                    movablePanel.IncomeConf();
                    Controls.Add(movablePanel);
                }
                movablePanel.BringToFront();
            };
            Controls.Add(openMovablePanelButton);

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

            foreach (var t in transactions)
            {
                transactionService.AddTransaction(t);
            }

            DateTime startDate = today.AddMonths(-11);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);
            
            // Update the chart
            budgetChart.SetTransactions(transactions);
            budgetChart.SetBudget(budget);
            budgetChart.SetDateRange(startDate, today);
            budgetChart.Invalidate();
            transactionHistory.SetTransactions(transactions);
            transactionHistory.Invalidate();
        }
    }
}