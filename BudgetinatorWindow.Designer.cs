namespace Budgetinator_2000;

partial class BudgetinatorWindow  
{

    private System.Windows.Forms.Button button1;
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.button1 = new System.Windows.Forms.Button();

        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.WindowState = FormWindowState.Maximized;
        this.Text = "Budgetinator 2000";

        this.button1.Text = "Boom";
        this.button1.Location = new System.Drawing.Point(50, 50);
        this.button1.Size = new System.Drawing.Size(100, 30);

         this.Controls.Add(this.button1);
    }

    #endregion
}
