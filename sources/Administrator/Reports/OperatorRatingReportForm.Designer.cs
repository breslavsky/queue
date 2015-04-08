﻿namespace Queue.Administrator.Reports
{
    partial class OperatorRatingReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.detailLevelTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.startYearComboBox = new System.Windows.Forms.ComboBox();
            this.finishYearComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.startMonthPicker = new System.Windows.Forms.DateTimePicker();
            this.finishMonthPicker = new System.Windows.Forms.DateTimePicker();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.finishDatePicker = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.isFullCheckBox = new System.Windows.Forms.CheckBox();
            this.operatorsListBox = new System.Windows.Forms.CheckedListBox();
            this.createReportButton = new System.Windows.Forms.Button();
            this.mainLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.detailLevelTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 2;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.mainLayoutPanel.Controls.Add(this.panel1, 1, 0);
            this.mainLayoutPanel.Controls.Add(this.panel2, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.operatorsListBox, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.createReportButton, 1, 2);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 3;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(765, 505);
            this.mainLayoutPanel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.detailLevelTabControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(555, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.mainLayoutPanel.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(210, 465);
            this.panel1.TabIndex = 0;
            // 
            // detailLevelTabControl
            // 
            this.detailLevelTabControl.Controls.Add(this.tabPage1);
            this.detailLevelTabControl.Controls.Add(this.tabPage2);
            this.detailLevelTabControl.Controls.Add(this.tabPage3);
            this.detailLevelTabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.detailLevelTabControl.Location = new System.Drawing.Point(0, 0);
            this.detailLevelTabControl.Name = "detailLevelTabControl";
            this.detailLevelTabControl.Padding = new System.Drawing.Point(5, 5);
            this.detailLevelTabControl.SelectedIndex = 0;
            this.detailLevelTabControl.Size = new System.Drawing.Size(210, 100);
            this.detailLevelTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.startYearComboBox);
            this.tabPage1.Controls.Add(this.finishYearComboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(202, 70);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Год";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // startYearComboBox
            // 
            this.startYearComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startYearComboBox.FormattingEnabled = true;
            this.startYearComboBox.Location = new System.Drawing.Point(10, 10);
            this.startYearComboBox.Name = "startYearComboBox";
            this.startYearComboBox.Size = new System.Drawing.Size(60, 21);
            this.startYearComboBox.TabIndex = 0;
            this.startYearComboBox.SelectedIndexChanged += new System.EventHandler(this.startYearComboBox_SelectedIndexChanged);
            // 
            // finishYearComboBox
            // 
            this.finishYearComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.finishYearComboBox.FormattingEnabled = true;
            this.finishYearComboBox.Location = new System.Drawing.Point(10, 40);
            this.finishYearComboBox.Name = "finishYearComboBox";
            this.finishYearComboBox.Size = new System.Drawing.Size(60, 21);
            this.finishYearComboBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.startMonthPicker);
            this.tabPage2.Controls.Add(this.finishMonthPicker);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(202, 70);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Месяц";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // startMonthPicker
            // 
            this.startMonthPicker.CustomFormat = "MMMM yyyy";
            this.startMonthPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startMonthPicker.Location = new System.Drawing.Point(10, 10);
            this.startMonthPicker.Name = "startMonthPicker";
            this.startMonthPicker.ShowUpDown = true;
            this.startMonthPicker.Size = new System.Drawing.Size(100, 20);
            this.startMonthPicker.TabIndex = 0;
            this.startMonthPicker.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            // 
            // finishMonthPicker
            // 
            this.finishMonthPicker.CustomFormat = "MMMM yyyy";
            this.finishMonthPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.finishMonthPicker.Location = new System.Drawing.Point(10, 40);
            this.finishMonthPicker.Name = "finishMonthPicker";
            this.finishMonthPicker.ShowUpDown = true;
            this.finishMonthPicker.Size = new System.Drawing.Size(100, 20);
            this.finishMonthPicker.TabIndex = 0;
            this.finishMonthPicker.Value = new System.DateTime(2013, 2, 10, 0, 0, 0, 0);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.startDatePicker);
            this.tabPage3.Controls.Add(this.finishDatePicker);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(202, 70);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "День";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // startDatePicker
            // 
            this.startDatePicker.Location = new System.Drawing.Point(10, 10);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(120, 20);
            this.startDatePicker.TabIndex = 0;
            // 
            // finishDatePicker
            // 
            this.finishDatePicker.Location = new System.Drawing.Point(10, 40);
            this.finishDatePicker.Name = "finishDatePicker";
            this.finishDatePicker.Size = new System.Drawing.Size(120, 20);
            this.finishDatePicker.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.isFullCheckBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(555, 30);
            this.panel2.TabIndex = 0;
            // 
            // isFullCheckBox
            // 
            this.isFullCheckBox.AutoSize = true;
            this.isFullCheckBox.Location = new System.Drawing.Point(5, 5);
            this.isFullCheckBox.Name = "isFullCheckBox";
            this.isFullCheckBox.Size = new System.Drawing.Size(133, 17);
            this.isFullCheckBox.TabIndex = 0;
            this.isFullCheckBox.Text = "По всем операторам";
            this.isFullCheckBox.UseVisualStyleBackColor = true;
            this.isFullCheckBox.CheckedChanged += new System.EventHandler(this.isFullCheckBox_CheckedChanged);
            // 
            // operatorsListBox
            // 
            this.operatorsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operatorsListBox.FormattingEnabled = true;
            this.operatorsListBox.Location = new System.Drawing.Point(3, 33);
            this.operatorsListBox.Name = "operatorsListBox";
            this.operatorsListBox.Size = new System.Drawing.Size(549, 429);
            this.operatorsListBox.TabIndex = 1;
            // 
            // createReportButton
            // 
            this.createReportButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.createReportButton.Location = new System.Drawing.Point(662, 472);
            this.createReportButton.Name = "createReportButton";
            this.createReportButton.Size = new System.Drawing.Size(100, 25);
            this.createReportButton.TabIndex = 0;
            this.createReportButton.Text = "Сформировать";
            this.createReportButton.Click += new System.EventHandler(this.createReportButton_Click);
            // 
            // OperatorRatingReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 505);
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "OperatorRatingReportForm";
            this.Text = "Отчет: рейтинг операторов";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OperatorRatingReportForm_FormClosing);
            this.Load += new System.EventHandler(this.OperatorRatingReportForm_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.detailLevelTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl detailLevelTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox startYearComboBox;
        private System.Windows.Forms.ComboBox finishYearComboBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DateTimePicker startMonthPicker;
        private System.Windows.Forms.DateTimePicker finishMonthPicker;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker finishDatePicker;
        private System.Windows.Forms.Button createReportButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox isFullCheckBox;
        private System.Windows.Forms.CheckedListBox operatorsListBox;
    }
}