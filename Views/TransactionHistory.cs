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
            Height = EstimateContentHeight();
            Invalidate();
        }
        private void OrderTransactions()
        {
           _transactions = _transactions.OrderByDescending(t => t.Date).ToList();
        }
        private int EstimateContentHeight()
        {
            int rowHeight = 25;
            int yPosition = 10;

            DateTime? currentMonth = null;
            DateTime? currentDay = null;

            foreach (var transaction in _transactions)
            {
                if (currentMonth == null || currentMonth.Value.Month != transaction.Date.Month
                    || currentMonth.Value.Year != transaction.Date.Year)
                {
                    currentMonth = transaction.Date;
                    currentDay = null;
                    yPosition += rowHeight;
                }

                if (currentDay == null || currentDay.Value.Day != transaction.Date.Day)
                {
                    currentDay = transaction.Date;
                    yPosition += rowHeight;
                }

                yPosition += rowHeight;
            }

            return yPosition + 10;
        }
        protected override void OnPaint(PaintEventArgs e){

            base.OnPaint(e);
            var g = e.Graphics;
            // Tool Box
            Font font = new Font("Arial", 12);
            Brush textBrush = new SolidBrush(Color.Black);
            Brush headerTextBrush = new SolidBrush(Color.White);
            Brush expenseBrsuh = new SolidBrush(Color.Red);
            Brush incomeBrush = new SolidBrush(Color.Green);

            int yPosition = 10;
            int rowHeight = 25;
            int correctX = 10;

            int categoryWidth = 200;
            int descriptionWidth = 300;
            int amountWidth = 100;
            int totalRowWidth = categoryWidth + descriptionWidth + amountWidth;

            float textHeight = g.MeasureString("A", font).Height;
            float textCounterY = (rowHeight - textHeight) / 2f;

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
                        g.FillRectangle(new SolidBrush(Color.FromArgb(50, 50, 50)), new Rectangle(correctX, yPosition, totalRowWidth, rowHeight));
                        g.DrawString(transaction.Date.ToString("MMMM yyyy"), font, headerTextBrush, correctX, yPosition  + textCounterY);

                        yPosition += rowHeight;

                    }
                if (currentDay == null || currentDay.Value.Day != transaction.Date.Day)
                    {
                        currentDay = transaction.Date;
                        g.FillRectangle(new SolidBrush(Color.FromArgb(80, 80, 80)), new Rectangle(correctX, yPosition, totalRowWidth, rowHeight));
                        g.DrawString(transaction.Date.ToString("dd/MM/yyyy"), font, headerTextBrush, correctX, yPosition + textCounterY);
                        yPosition += rowHeight;
                    }

                int xPosition = correctX;
                string categoryText = transaction.Category?.Name != null ? transaction.Category.Name :  "No Category";
                g.DrawString(categoryText, font, textBrush, new RectangleF(xPosition, yPosition + textCounterY, categoryWidth, rowHeight));
                
                SizeF categorySize = g.MeasureString(categoryText, font);
                xPosition += (int)categorySize.Width + 10;

                string description = transaction.Description != null ? transaction.Description :  "No Description";
                g.DrawString(description, font, textBrush, new RectangleF(xPosition, yPosition + textCounterY, descriptionWidth, rowHeight));
                SizeF descriptionSize = g.MeasureString(description, font);
                xPosition += (int)descriptionSize.Width + 40;
 
                Brush amountBrush = transaction.Type == TransactionType.Income ? incomeBrush : expenseBrsuh;
                string money = (transaction.Amount ?? 0).ToString("C");
                g.DrawString(money, font, amountBrush, new RectangleF(xPosition, yPosition + textCounterY, amountWidth, rowHeight));
                g.DrawLine(Pens.LightGray, correctX, yPosition + rowHeight - 1, correctX + totalRowWidth, yPosition + rowHeight - 1);
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