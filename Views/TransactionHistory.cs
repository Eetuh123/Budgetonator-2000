using System.Linq;
using Budgetinator_2000.Controls;
using Budgetinator_2000.Models;

namespace Budgetinator_2000.Views
{
    public partial class TransactionHistory : Control
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void SetTransactions(List<Transaction> transactions)
        {
            _transactions = transactions;
            OrderTransactions(); 
        }
        private void OrderTransactions()
        {
           _transactions = _transactions.OrderByDescending(t => t.Date).ToList();
        }
        protected override void OnPaint(PaintEventArgs e){

            base.OnPaint(e);
            var g = e.Graphics;
            // Tool Box
            Font font = new Font("Arial", 8);
            Brush textBrush = new SolidBrush(Color.Black);
            Brush expenseBrsuh = new SolidBrush(Color.Red);
            Brush incomeBrush = new SolidBrush(Color.Green);

            int yPosition = 10;
            int rowHeight = 25;

            int dateX = 10;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DateTime? currentMonth = null;
            DateTime? currentDay = null;

            foreach(var transaction in _transactions)
            {
                if (currentMonth == null || currentMonth.Value.Month != transaction.Date.Month
                    || currentMonth.Value.Year != transaction.Date.Year)
                    {
                        currentMonth = transaction.Date;
                        currentDay = null;
                        g.DrawString(transaction.Date.ToString("MMMM yyyy"), font, textBrush, dateX, yPosition);
                        yPosition += rowHeight;

                    }
                if (currentDay == null || currentDay.Value.Day != transaction.Date.Day)
                    {
                        currentDay = transaction.Date;
                        g.DrawString(transaction.Date.ToString("dd/MM/yyyy"), font, textBrush, dateX, yPosition);
                        yPosition += rowHeight;
                    }

                int xPosition = dateX;
                string categoryText = transaction.Category != null ? transaction.Category.Name :  "No Category";
                g.DrawString(categoryText, font, textBrush, xPosition, yPosition);
                
                SizeF categorySize = g.MeasureString(categoryText, font);
                xPosition += (int)categorySize.Width + 10;

                string description = transaction.Description != null ? transaction.Description :  "No Description";
                g.DrawString(description, font, textBrush, xPosition, yPosition);
                SizeF descriptionSize = g.MeasureString(description, font);
                xPosition += (int)descriptionSize.Width + 40;
 
                Brush amountBrush = transaction.Type == TransactionType.Income ? incomeBrush : expenseBrsuh;
                string money = (transaction.Amount ?? 0).ToString("C");
                g.DrawString(money, font, amountBrush , xPosition, yPosition);
                yPosition += rowHeight;
            }
            // Throw them away when not needed real enivoment friendly :)
            font.Dispose();
            textBrush.Dispose();
            expenseBrsuh.Dispose();
            incomeBrush.Dispose();
        }
    }
}