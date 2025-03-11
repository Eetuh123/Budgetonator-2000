namespace Budgetinator_2000.Controls
{
    public class MovablePanel : Panel  // Remove parentheses after class name
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


            // Create close button
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