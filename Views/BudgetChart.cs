using Budgetinator_2000.Models;

namespace Budgetinator_2000.Views
{
    public class BudgetChart : Control
    {
        private List<Transaction> _transactions = new List<Transaction>();
        private DateTime _startDate = new DateTime(DateTime.Today.AddMonths(-11).Year, DateTime.Today.AddMonths(-11).Month, 1);
        private DateTime _endDate = DateTime.Today;
        private float _maxMoney = 0;

        private Budget _budget = new Budget();
        
        private Color _incomeColor = Color.FromArgb(120, 200, 120);
        private Color _expenseColor = Color.FromArgb(220, 50, 50);
        private Color _targetLineColor = Color.FromArgb(255, 128, 0);
        
        public void SetTransactions(List<Transaction> transactions)
        {
            _transactions = transactions;
            CalculateMaxValue();
            Invalidate();
        }
        
        public void SetDateRange(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            CalculateMaxValue();
        }
        
        public void SetBudget(Budget budget)
        {
            _budget = budget;
        }
        
        private void CalculateMaxValue()
        {
            // If no transactions made we insert 1000 as max height
            if (_transactions == null || !_transactions.Any())
            {
                _maxMoney = 1000;
                return;
            }
            // We accsess MonthlyData Function and create list and we insert it into monthlyData var
            var monthlyData = GetMonthlyData();
            // We use LINQ to sort newly created list into decending order with maxium spending first 
            var monthWithMaxExpense = monthlyData.OrderByDescending(m => m.Expenses).First();
            // Now we compine Months Expenses and Income to get the total spendings for the month
            _maxMoney = (float)(monthWithMaxExpense.Income + monthWithMaxExpense.Expenses);
            // Creating our budget line
            if (_budget != null)
            {
                // Looping through each month 
                foreach (var month in monthlyData)
                {
                    decimal target = _budget.GetMonthlyTarget(month.YearMonth);
                    _maxMoney = Math.Max(_maxMoney, (float)target);
                }
            }
        }
        
        private List<MonthlyData> GetMonthlyData()
        {
            // Here we do a LINQ "query" to find all the transactions and sort them in to order by month and we create objects out of them and then we sort them chronologically convert into a list
            var monthlyGroups = _transactions
                .Where(t => t.Date >= _startDate && t.Date <= _endDate && t.Amount.HasValue)
                .GroupBy(t => new { Year = t.Date.Year, Month = t.Date.Month })
                .Select(g => new
                {
                    YearMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount ?? 0),
                    Expenses = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount ?? 0)
                })
                .OrderBy(m => m.YearMonth)
                .ToList();
            // We Create new empty list with all the months in there
            var allMonths = new List<MonthlyData>();
            // Here we store our initialized private var startDate and endDate so we can use them in upcoming while statment to go through whole year of data 
            var currentDate = new DateTime(_startDate.Year, _startDate.Month, 1);
            var endOfRange = new DateTime(_endDate.Year, _endDate.Month, 1);
            // We start the loop from 1 year ago to today
            while (currentDate <= endOfRange)
            {
                // Here we check if any of the monthlyGroups (where the transaction data is) list does it have any transaction matching the currentDates month if yes we store it
                var monthData = monthlyGroups.FirstOrDefault(m => m.YearMonth == currentDate);
                // Then we add it to our list we give it first month, Then income if its null then we use 0 and same for expenses
                allMonths.Add(new MonthlyData
                {
                    YearMonth = currentDate,
                    Income = monthData?.Income ?? 0,
                    Expenses = monthData?.Expenses ?? 0
                });
                // This takes our loop to next month
                currentDate = currentDate.AddMonths(1);
            }

            return allMonths;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            // calling onPaint for some painting
            base.OnPaint(e);
            
            // e.Graphics is what we use  for drawing lines
            var g = e.Graphics;
            // Here we enable AntiAlias for upcoming operations
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // We accsess MonthlyData Function and create list and we insert it into monthlyData var
            var monthlyData = GetMonthlyData();
            // In case its empty we cancel our drawing session
            if (!monthlyData.Any())
                return;
            // Initialize the drawing position bassically where to draw and how big
            int leftMargin = 30;
            int rightMargin = 10;
            int topMargin = 10;
            int bottomMargin = 25;
            int chartWidth = Width - leftMargin - rightMargin;
            int chartHeight = Height - topMargin - bottomMargin;
            
            // Our Drawing operations
            MakeMoneyLines(g, chartHeight, topMargin, leftMargin);
            DrawMoneyBars(g, monthlyData, leftMargin, chartWidth, chartHeight, topMargin);
            DrawTargetLines(g, monthlyData, leftMargin, chartWidth, chartHeight, topMargin);
            MakeTimeLine(g, monthlyData, leftMargin, chartWidth, chartHeight, topMargin);
        }
        
        private void MakeMoneyLines(Graphics g, int chartHeight, int yStart, int xStart)
        {
            // We divide our max money into 3 portions
            int numGridLines = 3; 
            float gridStep = _maxMoney / numGridLines;
            // Color font etc...
            using (var gridPen = new Pen(Color.FromArgb(220, 220, 220), 1))
            using (var font = new Font("Arial", 6))
            using (var brush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                // Loop for each line and where to draw it
                for (int i = 0; i <= numGridLines; i++)
                {

                    float yValue = i * gridStep;
                    float yPos = yStart + chartHeight - (chartHeight * (yValue / _maxMoney));
                    
                    g.DrawLine(gridPen, 0, yPos, Width, yPos);
                    // Figure which label to give
                    string label = "";
                    if (yValue >= 1000000)
                        label = (yValue / 1000000).ToString("0") + "M";
                    else if (yValue >= 1000)
                        label = (yValue / 1000).ToString("0") + "k";
                    else
                        label = yValue.ToString("0");
                        
                    var textSize = g.MeasureString(label, font);
                    g.DrawString(label, font, brush, xStart - textSize.Width - 5, yPos - (textSize.Height / 2));
                }
            }
        }
        
        private void MakeTimeLine(Graphics g, List<MonthlyData> monthlyData, int leftMargin, int chartWidth, int chartHeight, int yStart)
        {
            using (var font = new Font("Arial", 6))
            using (var brush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                float barWidth = chartWidth / (float)monthlyData.Count;
                
                for (int i = 0; i < monthlyData.Count; i++)
                {
                    float xPos = leftMargin + (i * barWidth) + (barWidth / 2);
                    string label = monthlyData[i].YearMonth.ToString("MMM");
                    
                    var textSize = g.MeasureString(label, font);
                    g.DrawString(label, font, brush, xPos - (textSize.Width / 2), yStart + chartHeight + 5);
                }
            }
        }
        
        private void DrawMoneyBars(Graphics g, List<MonthlyData> monthlyData, int margin, int chartWidth, int chartHeight, int yStart)
        {
            float barWidth = chartWidth / (float)monthlyData.Count;
            float barInnerWidth = barWidth * 0.5f;
            float barMargin = (barWidth - barInnerWidth) / 2;
            
            using (var incomeBrush = new SolidBrush(_incomeColor))
            using (var expenseBrush = new SolidBrush(_expenseColor))
            {
                for (int i = 0; i < monthlyData.Count; i++)
                {
                    float xPos = margin + (i * barWidth) + barMargin;
                    var data = monthlyData[i];
                    
                    float incomeHeight = chartHeight * ((float)data.Income / _maxMoney);
                    float expenseHeight = chartHeight * ((float)data.Expenses / _maxMoney);
                    
                    g.FillRectangle(incomeBrush,
                        xPos,
                        yStart + chartHeight - incomeHeight,
                        barInnerWidth,
                        incomeHeight);
                    
                    g.FillRectangle(expenseBrush,
                        xPos,
                        yStart + chartHeight - incomeHeight - expenseHeight,
                        barInnerWidth,
                        expenseHeight);
                }
            }
        }
        
        private void DrawTargetLines(Graphics g, List<MonthlyData> monthlyData, int margin, int chartWidth, int chartHeight, int yStart)
        {
            if (_budget == null)
                return;
                
            float barWidth = chartWidth / (float)monthlyData.Count;
            // Calculations for positions of the budget Targets
            using (var targetPen = new Pen(_targetLineColor, 2))
            {
                var points = new List<PointF>();
                
                for (int i = 0; i < monthlyData.Count; i++)
                {
                    var data = monthlyData[i];
                    
                    float xPos = margin + (i * barWidth) + (barWidth / 2);
                    
                    decimal target = _budget.GetMonthlyTarget(data.YearMonth);
                    
                    float targetHeight = chartHeight * ((float)target / _maxMoney);
                    float yPos = yStart + chartHeight - targetHeight;
                    
                    points.Add(new PointF(xPos, yPos));
                }
                // For making the lines
                if (points.Count > 1)
                {
                    g.DrawLines(targetPen, points.ToArray());
                }
                // For painting the little dots
                using (var targetBrush = new SolidBrush(_targetLineColor))
                {
                    foreach (var point in points)
                    {
                        g.FillEllipse(targetBrush, point.X - 3, point.Y - 3, 6, 6);
                    }
                }
            }
        }
        
        private class MonthlyData
        {
            public DateTime YearMonth { get; set; }
            public decimal Income { get; set; }
            public decimal Expenses { get; set; }
        }
    }
}