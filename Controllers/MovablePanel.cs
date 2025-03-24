using System;
using System.Drawing;
using System.Windows.Forms;
using Budgetinator_2000.Views;
using System.Globalization;
using Budgetinator_2000.Models;
using Budgetinator_2000.Mathcalculations;

namespace Budgetinator_2000.Controls
{
    public class MovablePanel : Panel
    {
        private bool isDragging = false;
        private Point dragStartPoint;
        private Button closeButton;
        private DateTimePicker datePicker;
        private Label dateLabel;
        private DateTimePicker endDatePicker;
        private Label endDateLabel;
        private TransactionHistory transactionHistory;
        private BudgetChart budgetChart;

        private readonly TransactionService transactionService;

        public void SetTransactionHistory(TransactionHistory history)
        {
            transactionHistory = history;
        }
        public void SetBudgetChart(BudgetChart chart)
        {
            budgetChart = chart;
        }
        public MovablePanel(TransactionService sharedService)
        {
            transactionService = sharedService;
            datePicker = new DateTimePicker();
            dateLabel = new Label();
            endDatePicker = new DateTimePicker();
            endDateLabel = new Label();

            Size = new Size(400, 450);
            Location = new Point(100, 100);
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.DarkGray;

            MouseDown += MovablePanel_MouseDown;
            MouseMove += MovablePanel_MouseMove;
            MouseUp += MovablePanel_MouseUp;

            closeButton = new Button
            {
                Text = "X",
                Size = new Size(25, 25),
                Location = new Point(Width - 30, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            Controls.Add(closeButton);
            closeButton.Click += CloseButton_Click;
        }

        public void IncomeConf()
        {
            // Name TextBox
            TextBox nameTextBox = new TextBox
            {
                Text = "Name",
                Size = new Size(150, 30),
                Location = new Point(30, 50),
                BackColor = Color.LightGray,
                Visible = false
            };

            nameTextBox.GotFocus += (sender, e) =>
            {
                if (nameTextBox.Text == "Name")
                {
                    nameTextBox.Text = "";
                }
            };

            nameTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    nameTextBox.Text = "Name";
                }
            };
            Controls.Add(nameTextBox);

            // Description TextBox
            TextBox descriptionTextBox = new TextBox
            {
                Text = "Transaction name",
                Size = new Size(150, 30),
                Location = new Point(30, 50),
                BackColor = Color.LightGray
            };

            descriptionTextBox.GotFocus += (sender, e) =>
            {
                if (descriptionTextBox.Text == "Transaction name")
                {
                    descriptionTextBox.Text = "";
                }
            };

            descriptionTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
                {
                    descriptionTextBox.Text = "Transaction name";
                }
            };
            Controls.Add(descriptionTextBox);
            // Date Label alkuperäiselle päivämäärälle
            dateLabel = new Label
            {
                Text = "Date:",
                Size = new Size(60, 30),
                Location = new Point(30, 95),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(dateLabel);

            // DateTimePicker alkuperäiselle päivämäärälle
            datePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Size = new Size(150, 30),
                Location = new Point(30, dateLabel.Bottom + 5)
            };
            Controls.Add(datePicker);

            // Label lopetuspäivämäärälle
            endDateLabel = new Label
            {
                Text = "End Date:",
                Size = new Size(70, 30),
                Location = new Point(220, 95),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };
            Controls.Add(endDateLabel);

            // DateTimePicker lopetuspäivämäärälle
            endDatePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Size = new Size(150, 30),
                Location = new Point(220, endDateLabel.Bottom + 5),
                Visible = false
            };
            Controls.Add(endDatePicker);


            // Category TextBox
            ComboBox categoryComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(150, 30),
                Location = new Point(30, endDatePicker.Bottom + 20),
                BackColor = Color.LightGray
            };
            categoryComboBox.Items.Add("Pick category");
            categoryComboBox.Items.AddRange(new object[] {
             "Housing",
             "Groceries",
             "Transportation",
             "Bills & Utilities",
             "Entertainment & Leisure",
             "Clothing & Personal Care",
             "Healthcare",
             "Savings & Investments",
             "Dining Out",
             "Salary",
             "Rent or Mortgage",
             "Utilities",
             "Hobbies",
             "Travel",
             "Other"
            });
            categoryComboBox.SelectedIndex = 0;

            Controls.Add(categoryComboBox);

            // Repeat CheckBox
            CheckBox repeatCheckBox = new CheckBox
            {
                Text = "Repeat?",
                Size = new Size(80, 30),
                Location = new Point(220, endDatePicker.Bottom + 20),
                BackColor = Color.DarkGray,
                ForeColor = Color.White
            };
            Controls.Add(repeatCheckBox);

            // Weekly / Monthly / Yearly Buttons
            Button weeklyButton = new Button
            {
                Text = "Weekly",
                Size = new Size(100, 30),
                Location = new Point(30, repeatCheckBox.Bottom + 10),
                BackColor = Color.LightGray,
                Visible = false
            };
            Button monthlyButton = new Button
            {
                Text = "Monthly",
                Size = new Size(100, 30),
                Location = new Point(weeklyButton.Right + 10, weeklyButton.Top),
                BackColor = Color.LightGray,
                Visible = false
            };
            Button yearlyButton = new Button
            {
                Text = "Yearly",
                Size = new Size(100, 30),
                Location = new Point(monthlyButton.Right + 10, weeklyButton.Top),
                BackColor = Color.LightGray,
                Visible = false
            };
            Controls.Add(weeklyButton);
            Controls.Add(monthlyButton);
            Controls.Add(yearlyButton);


            // Kun valitsee minkä mukaan toistuu se maalaa kyseisen napin ja ei anna valita useampaa samaan aikaan. voimistaa valitun napin värin
            bool isWeeklyButtonToggled = false;
            bool isMonthlyButtonToggled = false;
            bool isYearlyButtonToggled = false;

            weeklyButton.Click += (sender, e) =>
            {
                isWeeklyButtonToggled = !isWeeklyButtonToggled;
                if (isWeeklyButtonToggled)
                {
                    isMonthlyButtonToggled = false;
                    isYearlyButtonToggled = false;
                    monthlyButton.BackColor = Color.LightGray;
                    yearlyButton.BackColor = Color.LightGray;

                    weeklyButton.BackColor = Color.Gray;
                }
                else
                {
                    weeklyButton.BackColor = Color.LightGray;
                }
            };

            monthlyButton.Click += (sender, e) =>
            {
                isMonthlyButtonToggled = !isMonthlyButtonToggled;
                if (isMonthlyButtonToggled)
                {
                    isWeeklyButtonToggled = false;
                    isYearlyButtonToggled = false;
                    weeklyButton.BackColor = Color.LightGray;
                    yearlyButton.BackColor = Color.LightGray;

                    monthlyButton.BackColor = Color.Gray;
                }
                else
                {
                    monthlyButton.BackColor = Color.LightGray;
                }
            };

            yearlyButton.Click += (sender, e) =>
            {
                isYearlyButtonToggled = !isYearlyButtonToggled;
                if (isYearlyButtonToggled)
                {
                    isWeeklyButtonToggled = false;
                    isMonthlyButtonToggled = false;
                    weeklyButton.BackColor = Color.LightGray;
                    monthlyButton.BackColor = Color.LightGray;

                    yearlyButton.BackColor = Color.Gray;
                }
                else
                {
                    yearlyButton.BackColor = Color.LightGray;
                }
            };

            // Kun checkbox-tilanne muuttuu, vaihdetaan kolmen napin Visible-arvo
            repeatCheckBox.CheckedChanged += (sender, e) =>
            {
                bool isChecked = repeatCheckBox.Checked;
                weeklyButton.Visible = isChecked;
                monthlyButton.Visible = isChecked;
                yearlyButton.Visible = isChecked;
                endDateLabel.Visible = isChecked;
                endDatePicker.Visible = isChecked;
            };

            // Amount TextBox
            TextBox amountTextBox = new TextBox
            {
                Text = "Enter Amount",
                Size = new Size(300, 30),
                Location = new Point(30, weeklyButton.Bottom + 20),
                BackColor = Color.LightGray,
                TextAlign = HorizontalAlignment.Center
            };
            Controls.Add(amountTextBox);

            amountTextBox.GotFocus += (sender, e) =>
            {
                if (amountTextBox.Text == "Enter Amount")
                {
                    amountTextBox.Text = "";
                }
            };

            amountTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(amountTextBox.Text))
                {
                    amountTextBox.Text = "Enter Amount";
                }
            };

            // sallii vain numerot ja yhden pilkun, jonka jälkeen 2 numeroa
            amountTextBox.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == ' ')
                {
                    e.Handled = true;
                    return;
                }

                if (char.IsControl(e.KeyChar))
                    return;

                string text = amountTextBox.Text;
                //Jos pilkku on eka niin lisää 0 sen eteen ja siirtää cursorin pilkun jälkeen
                if (text.Length == 0 && e.KeyChar == ',')
                {
                    amountTextBox.Text = "0,";
                    e.Handled = true;
                    amountTextBox.SelectionStart = amountTextBox.Text.Length;
                    return;
                }

                if (text.Contains(','))
                {
                    if (e.KeyChar == ',')
                    {
                        e.Handled = true;
                        return;
                    }

                    int indexAfterComma = text.IndexOf(',') + 1;
                    int decimalsAfterComma = text.Length - indexAfterComma;

                    if (decimalsAfterComma >= 2)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (!char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.KeyChar == ',')
                    {
                        return;
                    }

                    if (!char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
            };

            CheckBox expenseCheckBox = new CheckBox
            {
                Text = "Expense",
                Size = new Size(100, 30),
                Location = new Point(amountTextBox.Left, amountTextBox.Bottom + 10),
                BackColor = Color.DarkGray,
                ForeColor = Color.White
            };
            Controls.Add(expenseCheckBox);

            CheckBox incomeCheckBox = new CheckBox
            {
                Text = "income",
                Size = new Size(100, 30),
                Location = new Point(amountTextBox.Left, amountTextBox.Bottom + 30),
                BackColor = Color.DarkGray,
                ForeColor = Color.White
            };
            Controls.Add(incomeCheckBox);

            CheckBox grossCheckBox = new CheckBox
            {
                Text = "gross",
                Size = new Size(80, 30),
                Location = new Point(amountTextBox.Left, amountTextBox.Bottom + 10),
                BackColor = Color.DarkGray,
                ForeColor = Color.White,
                Visible = false
            };
            Controls.Add(grossCheckBox);

            CheckBox netCheckBox = new CheckBox
            {
                Text = "Net",
                Size = new Size(80, 30),
                Location = new Point(amountTextBox.Left, amountTextBox.Bottom + 30),
                BackColor = Color.DarkGray,
                ForeColor = Color.White,
                Visible = false
            };
            Controls.Add(netCheckBox);

            TextBox netTaxTextBox = new TextBox
            {
                Text = "Enter Tax %",
                Size = new Size(70, 30),
                Location = new Point(netCheckBox.Right, amountTextBox.Bottom + 35),
                BackColor = Color.LightGray,
                Visible = false
            };
            Controls.Add(netTaxTextBox);

            // pilotee asioita ja laittaa esille
            categoryComboBox.SelectedIndexChanged += (sender, e) =>
            {
                if (categoryComboBox.SelectedItem != null && categoryComboBox.SelectedItem.ToString() == "Salary")
                {
                    grossCheckBox.Visible = true;
                    netCheckBox.Visible = true;
                    netTaxTextBox.Visible = netCheckBox.Checked;
                    expenseCheckBox.Visible = false;
                    incomeCheckBox.Visible = false;
                    netCheckBox.Checked = false;
                    grossCheckBox.Checked = false;
                }
                else
                {
                    grossCheckBox.Visible = false;
                    netCheckBox.Visible = false;
                    netCheckBox.Checked = false;
                    grossCheckBox.Checked = false;
                    expenseCheckBox.Visible = true;
                    incomeCheckBox.Visible = true;
                }
            };

            expenseCheckBox.CheckedChanged += (sender, e) =>
            {
                if (expenseCheckBox.Checked)
                {
                    incomeCheckBox.Checked = false;
                }
            };

            incomeCheckBox.CheckedChanged += (sender, e) =>
            {
                if (incomeCheckBox.Checked)
                {
                    expenseCheckBox.Checked = false;
                }
            };

            grossCheckBox.CheckedChanged += (sender, e) =>
            {
                if (grossCheckBox.Checked)
                {
                    netCheckBox.Checked = false;
                }
            };

            netCheckBox.CheckedChanged += (sender, e) =>
            {
                if (netCheckBox.Checked)
                {
                    grossCheckBox.Checked = false;
                    netTaxTextBox.Visible = true;
                }
                else
                {
                    netTaxTextBox.Visible = false;
                }
            };

            // Kun TextBox saa fokuksen, jos siellä on "Enter Tax %", vaihdetaan se pelkkään "%".
            // Tai jos se jo päättyy merkkiin '%', poistetaan se muokkausta varten.
            netTaxTextBox.GotFocus += (sender, e) =>
            {
                if (netTaxTextBox.Text == "Enter Tax %")
                {
                    netTaxTextBox.Text = "";
                }
                else if (netTaxTextBox.Text.EndsWith("%"))
                {
                    netTaxTextBox.Text = netTaxTextBox.Text.TrimEnd('%');
                }
            };

            // Kun TextBox menettää fokuksen, jos siellä on tyhjää,
            // palautetaan "Enter Tax %". Muuten lisätään "%", jos sitä ei ole.
            netTaxTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(netTaxTextBox.Text))
                {
                    netTaxTextBox.Text = "Enter Tax %";
                }
                else
                {
                    if (!netTaxTextBox.Text.EndsWith("%"))
                    {
                        netTaxTextBox.Text += "%";
                    }
                }
            };

            // Sallitaan vain numeroita ja yksi pilkku (kaksi desimaalia). Estetään välilyönnit.
            netTaxTextBox.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == ' ')
                {
                    e.Handled = true;
                    return;
                }

                if (char.IsControl(e.KeyChar))
                    return;

                string textWithoutPercent = netTaxTextBox.Text.Replace("%", "");

                if (textWithoutPercent.Length == 0 && e.KeyChar == ',')
                {
                    netTaxTextBox.Text = "0,";
                    e.Handled = true;
                    netTaxTextBox.SelectionStart = netTaxTextBox.Text.Length;
                    return;
                }

                if (textWithoutPercent.Contains(','))
                {
                    if (e.KeyChar == ',')
                    {
                        e.Handled = true;
                        return;
                    }

                    int indexAfterComma = textWithoutPercent.IndexOf(',') + 1;
                    int decimalsAfterComma = textWithoutPercent.Length - indexAfterComma;

                    if (decimalsAfterComma >= 2)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (!char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if (e.KeyChar == ',')
                    {
                        return;
                    }

                    if (!char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
            };

            // Submit Button
            Button submitButton = new Button
            {
                Text = "Add money",
                Size = new Size(200, 30),
                Location = new Point((Width - 200) / 2, netTaxTextBox.Bottom + 30), // Sijoitetaan alemmas
                BackColor = Color.LightGreen
            };
            Controls.Add(submitButton);

            submitButton.Click += (sender, e) =>
            {
                // Tarkistetaan Name‐kenttä
                if (string.IsNullOrWhiteSpace(descriptionTextBox.Text) || descriptionTextBox.Text == "Transaction name")
                {
                    MessageBox.Show("Please enter a valid Name.");
                    return;
                }

                // Tarkistetaan Category
                if (categoryComboBox.SelectedItem == null || categoryComboBox.SelectedItem.ToString() == "Pick category")
                {
                    MessageBox.Show("Please choose a Category.");
                    return;
                }

                // Tarkistetaan, onko repeat valittuna ja onko jokin napeista togglattuna
                if (repeatCheckBox.Checked)
                {
                    if (!isWeeklyButtonToggled && !isMonthlyButtonToggled && !isYearlyButtonToggled)
                    {
                        MessageBox.Show("Please select Weekly, Monthly, or Yearly.");
                        return;
                    }
                }

                // Tarkistetaan Amount
                if (!decimal.TryParse(amountTextBox.Text.Replace(',', '.'),
                                      NumberStyles.AllowDecimalPoint,
                                      CultureInfo.InvariantCulture,
                                      out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid Amount (greater than 0).");
                    return;
                }

                // Päätellään TransactionType sen perusteella onko "Salary" vai ei.
                TransactionType type;

                // Jos category on "Salary", transaction on automaattisesti Income
                if (categoryComboBox.SelectedItem.ToString() == "Salary")
                {
                    if (!grossCheckBox.Checked && !netCheckBox.Checked)
                    {
                        MessageBox.Show("Please select Gross or Net for 'Salary'.");
                        return;
                    }
                    // Asetetaan type suoraan Income
                    type = TransactionType.Income;
                }
                else
                {
                    if (!expenseCheckBox.Checked && !incomeCheckBox.Checked)
                    {
                        MessageBox.Show("Please select Expense or income.");
                        return;
                    }

                    if (expenseCheckBox.Checked)
                    {
                        type = TransactionType.Expense;
                    }
                    else
                    {
                        type = TransactionType.Income;
                    }
                }


                decimal finalAmount = amount;
                if (netCheckBox.Checked)
                {
                    string taxTextWithoutPercent = netTaxTextBox.Text.Replace("%", "");
                    if (!decimal.TryParse(taxTextWithoutPercent.Replace(',', '.'),
                                          NumberStyles.AllowDecimalPoint,
                                          CultureInfo.InvariantCulture,
                                          out decimal taxPercent) || taxPercent < 0)
                    {
                        MessageBox.Show("Please enter valid Tax %.");
                        return;
                    }
                    if (taxPercent < 0 || taxPercent > 100)
                    {
                        MessageBox.Show("Tax percentage must be between 0 and 100.");
                        return;
                    }
                    decimal taxDeduction = SalaryCalculations.TaxDeduction(amount, taxPercent);
                    finalAmount = SalaryCalculations.NetSalary(amount, taxDeduction);
                }

                var newTransaction = new Transaction
                {
                    Date = datePicker.Value,
                    Amount = finalAmount,
                    Category = new Category { Name = categoryComboBox.SelectedItem.ToString() },
                    Description = descriptionTextBox.Text,
                    Type = type
                };

                transactionService.AddTransaction(newTransaction); 

                if (repeatCheckBox.Checked && isWeeklyButtonToggled)
                {
                    DateTime startDate = datePicker.Value;
                    DateTime endDate = endDatePicker.Value;

                    if (startDate > endDate)
                    {
                        MessageBox.Show("End date must be greater than Start date");
                        return;
                    }

                    for (DateTime currentDate = startDate.AddDays(7); currentDate <= endDate; currentDate = currentDate.AddDays(7))
                    {
                        var recurringTransaction = new Transaction
                        {
                            Date = currentDate,
                            Amount = finalAmount,
                            Category = new Category { Name = categoryComboBox.SelectedItem.ToString() },
                            Description = descriptionTextBox.Text + " (Weekly)",
                            Type = type
                        };
                        transactionService.AddTransaction(recurringTransaction);
                    }
                }
                else if (repeatCheckBox.Checked && isMonthlyButtonToggled)
                {
                    DateTime startDate = datePicker.Value;
                    DateTime endDate = endDatePicker.Value;

                    if (startDate > endDate)
                    {
                        MessageBox.Show("End date must be greater than Start date");
                        return;
                    }

                    for (DateTime currentDate = startDate.AddMonths(1); currentDate <= endDate; currentDate = currentDate.AddMonths(1))
                    {
                        var recurringTransaction = new Transaction
                        {
                            Date = currentDate,
                            Amount = finalAmount,
                            Category = new Category { Name = categoryComboBox.SelectedItem.ToString() },
                            Description = descriptionTextBox.Text + " (Monthly)",
                            Type = type
                        };
                        transactionService.AddTransaction(recurringTransaction);
                    }
                }
                else if (repeatCheckBox.Checked && isYearlyButtonToggled)
                {
                    DateTime startDate = datePicker.Value;
                    DateTime endDate = endDatePicker.Value;

                    if (startDate > endDate)
                    {
                        MessageBox.Show("End date must be greater than Start date");
                        return;
                    }

                    for (DateTime currentDate = startDate.AddYears(1); currentDate <= endDate; currentDate = currentDate.AddYears(1))
                    {
                        var recurringTransaction = new Transaction
                        {
                            Date = currentDate,
                            Amount = finalAmount,
                            Category = new Category { Name = categoryComboBox.SelectedItem.ToString() },
                            Description = descriptionTextBox.Text + " (Yearly)",
                            Type = type
                        };
                        transactionService.AddTransaction(recurringTransaction);
                    }
                }

                // Päivitetään näkymät
                budgetChart.SetDateRange(
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-11),
                    DateTime.Today
                );
                budgetChart.SetTransactions(transactionService.GetTransactions());
                budgetChart.Invalidate();
                budgetChart.Update();
                transactionHistory.SetTransactions(transactionService.GetTransactions());
                transactionHistory.Invalidate();

                MessageBox.Show("Transaction added!");
            };
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            if (Parent != null)
            {
                Parent.Controls.Remove(this);
            }
            Dispose();
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
                Point newLocation = Location;
                newLocation.X += e.X - dragStartPoint.X;
                newLocation.Y += e.Y - dragStartPoint.Y;
                Location = newLocation;
            }
        }

        private void MovablePanel_MouseUp(object? sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}
