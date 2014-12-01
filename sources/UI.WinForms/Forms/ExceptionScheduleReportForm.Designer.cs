namespace Queue.UI.WinForms
{
    partial class ExceptionScheduleReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fromDateLabel = new System.Windows.Forms.Label();
            this.fromDateCalendar = new System.Windows.Forms.MonthCalendar();
            this.createReportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fromDateLabel
            // 
            this.fromDateLabel.AutoSize = true;
            this.fromDateLabel.Location = new System.Drawing.Point(10, 10);
            this.fromDateLabel.Name = "fromDateLabel";
            this.fromDateLabel.Size = new System.Drawing.Size(139, 13);
            this.fromDateLabel.TabIndex = 0;
            this.fromDateLabel.Text = "Выберите начальную дату";
            // 
            // fromDateCalendar
            // 
            this.fromDateCalendar.Location = new System.Drawing.Point(5, 25);
            this.fromDateCalendar.MaxSelectionCount = 1;
            this.fromDateCalendar.MinDate = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.fromDateCalendar.Name = "fromDateCalendar";
            this.fromDateCalendar.TabIndex = 0;
            // 
            // createReportButton
            // 
            this.createReportButton.Location = new System.Drawing.Point(70, 190);
            this.createReportButton.Name = "createReportButton";
            this.createReportButton.Size = new System.Drawing.Size(100, 25);
            this.createReportButton.TabIndex = 0;
            this.createReportButton.Text = "Сформировать";
            this.createReportButton.UseVisualStyleBackColor = true;
            this.createReportButton.Click += new System.EventHandler(this.createReportButton_Click);
            // 
            // ServiceExceptionScheduleReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 222);
            this.Controls.Add(this.createReportButton);
            this.Controls.Add(this.fromDateCalendar);
            this.Controls.Add(this.fromDateLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(190, 260);
            this.Name = "ServiceExceptionScheduleReportForm";
            this.Text = "Отчет: исключения в расписании";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExceptionScheduleReportForm_FormClosing);
            this.Load += new System.EventHandler(this.ExceptionScheduleReportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label fromDateLabel;
        private System.Windows.Forms.MonthCalendar fromDateCalendar;
        private System.Windows.Forms.Button createReportButton;

    }
}