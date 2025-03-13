namespace Budgetinator_2000.Controls
{
    public class MovablePanel : Panel
    {
        private bool isDragging = false;
        private Point dragStartPoint;
        private Button closeButton;

        public MovablePanel()
        {
            Size = new Size(400, 350);
            Location = new Point(100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightGray;

            this.MouseDown += MovablePanel_MouseDown;
            this.MouseMove += MovablePanel_MouseMove;
            this.MouseUp += MovablePanel_MouseUp;

            closeButton = new Button
            {
                Text = "X",
                Size = new Size(25, 25),
                Location = new Point(this.Width - 30, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            this.Controls.Add(closeButton);
            
            closeButton.Click += CloseButton_Click;
        }

        public void IncomeConf()
        {
            TextBox Name = new TextBox
            {
               Text = "Name",
               Size = new Size(120,30),
               Location = new Point((400 - 120) / 4, 50),
               BackColor = Color.DarkGray,

            };
            TextBox Description = new TextBox
            {
               Text = "Description",
               Size = new Size(120,30),
               Location = new Point(120, 50),
               BackColor = Color.DarkGray,

            };
            TextBox Gategory = new TextBox
            {
               Text = "Gategory",
               Size = new Size(120,30),
               Location = new Point(120, 50),
               BackColor = Color.DarkGray,

            };
            Label dateLabel = new Label
            {
                Text = "Date:",
                Size = new Size(120, 30),
                Location = new Point((400 - 120) / 2, 170),
                TextAlign = ContentAlignment.MiddleCenter
            };
            DateTimePicker datePicker = new DateTimePicker
            {
                Location = new Point((400 - 120) / 2, 200),
                Format = DateTimePickerFormat.Short
            };
            Button Submit = new Button
            {
               Text = "Add Income",
               Size = new Size(120,30),
               Location = new Point((400 - 120) / 2, 300) ,
               BackColor = Color.Green,

            };

            this.Controls.Add(dateLabel);
            this.Controls.Add(datePicker);
            this.Controls.Add(Name);
            this.Controls.Add(Description);
            this.Controls.Add(Submit);
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {

            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
            this.Dispose();
        }

        private void MovablePanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = e.Location;
            }
        }

        private void MovablePanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - dragStartPoint.X;
                newLocation.Y += e.Y - dragStartPoint.Y;
                this.Location = newLocation;
            }
        }

        private void MovablePanel_MouseUp(object? sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}