namespace Queue.Administrator
{
    partial class DefaultScheduleForm
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
            this.exceptionTabPage = new System.Windows.Forms.TabPage();
            this.exceptionScheduleControl = new Queue.Administrator.ScheduleControl();
            this.exceptionScheduleDatePicker = new System.Windows.Forms.DateTimePicker();
            this.exceptionScheduleCheckBox = new System.Windows.Forms.CheckBox();
            this.weekdayTabPage = new System.Windows.Forms.TabPage();
            this.weekdayTabControl = new System.Windows.Forms.TabControl();
            this.mondayTabPage = new System.Windows.Forms.TabPage();
            this.weekdayScheduleControl = new Queue.Administrator.ScheduleControl();
            this.tuesdayTabPage = new System.Windows.Forms.TabPage();
            this.wednesdayTabPage = new System.Windows.Forms.TabPage();
            this.thursdayTabPage = new System.Windows.Forms.TabPage();
            this.fridayTabPage = new System.Windows.Forms.TabPage();
            this.saturdayTabPage = new System.Windows.Forms.TabPage();
            this.sundayTabPage = new System.Windows.Forms.TabPage();
            this.scheduleTabControl = new System.Windows.Forms.TabControl();
            this.exceptionTabPage.SuspendLayout();
            this.weekdayTabPage.SuspendLayout();
            this.weekdayTabControl.SuspendLayout();
            this.mondayTabPage.SuspendLayout();
            this.scheduleTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // exceptionTabPage
            // 
            this.exceptionTabPage.Controls.Add(this.exceptionScheduleControl);
            this.exceptionTabPage.Controls.Add(this.exceptionScheduleDatePicker);
            this.exceptionTabPage.Controls.Add(this.exceptionScheduleCheckBox);
            this.exceptionTabPage.Location = new System.Drawing.Point(4, 26);
            this.exceptionTabPage.Name = "exceptionTabPage";
            this.exceptionTabPage.Padding = new System.Windows.Forms.Padding(5);
            this.exceptionTabPage.Size = new System.Drawing.Size(826, 396);
            this.exceptionTabPage.TabIndex = 0;
            this.exceptionTabPage.Text = "Исключения в расписании";
            this.exceptionTabPage.UseVisualStyleBackColor = true;
            // 
            // exceptionScheduleControl
            // 
            this.exceptionScheduleControl.Location = new System.Drawing.Point(15, 65);
            this.exceptionScheduleControl.Name = "exceptionScheduleControl";
            this.exceptionScheduleControl.Schedule = null;
            this.exceptionScheduleControl.Size = new System.Drawing.Size(790, 310);
            this.exceptionScheduleControl.TabIndex = 0;
            // 
            // exceptionScheduleDatePicker
            // 
            this.exceptionScheduleDatePicker.Location = new System.Drawing.Point(15, 15);
            this.exceptionScheduleDatePicker.Name = "exceptionScheduleDatePicker";
            this.exceptionScheduleDatePicker.Size = new System.Drawing.Size(180, 20);
            this.exceptionScheduleDatePicker.TabIndex = 0;
            this.exceptionScheduleDatePicker.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.exceptionScheduleDatePicker.ValueChanged += new System.EventHandler(this.exceptionScheduleDatePicker_ValueChanged);
            // 
            // exceptionScheduleCheckBox
            // 
            this.exceptionScheduleCheckBox.AutoSize = true;
            this.exceptionScheduleCheckBox.Location = new System.Drawing.Point(25, 45);
            this.exceptionScheduleCheckBox.Name = "exceptionScheduleCheckBox";
            this.exceptionScheduleCheckBox.Size = new System.Drawing.Size(150, 17);
            this.exceptionScheduleCheckBox.TabIndex = 0;
            this.exceptionScheduleCheckBox.Tag = "2";
            this.exceptionScheduleCheckBox.Text = "Определить расписание";
            this.exceptionScheduleCheckBox.UseVisualStyleBackColor = true;
            this.exceptionScheduleCheckBox.CheckedChanged += new System.EventHandler(this.exceptionScheduleCheckBox_CheckedChanged);
            this.exceptionScheduleCheckBox.Click += new System.EventHandler(this.exceptionScheduleCheckBox_Click);
            // 
            // weekdayTabPage
            // 
            this.weekdayTabPage.Controls.Add(this.weekdayTabControl);
            this.weekdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.weekdayTabPage.Margin = new System.Windows.Forms.Padding(5);
            this.weekdayTabPage.Name = "weekdayTabPage";
            this.weekdayTabPage.Padding = new System.Windows.Forms.Padding(5);
            this.weekdayTabPage.Size = new System.Drawing.Size(826, 396);
            this.weekdayTabPage.TabIndex = 0;
            this.weekdayTabPage.Text = "Регулярное расписание";
            this.weekdayTabPage.UseVisualStyleBackColor = true;
            // 
            // weekdayTabControl
            // 
            this.weekdayTabControl.Controls.Add(this.mondayTabPage);
            this.weekdayTabControl.Controls.Add(this.tuesdayTabPage);
            this.weekdayTabControl.Controls.Add(this.wednesdayTabPage);
            this.weekdayTabControl.Controls.Add(this.thursdayTabPage);
            this.weekdayTabControl.Controls.Add(this.fridayTabPage);
            this.weekdayTabControl.Controls.Add(this.saturdayTabPage);
            this.weekdayTabControl.Controls.Add(this.sundayTabPage);
            this.weekdayTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weekdayTabControl.Location = new System.Drawing.Point(5, 5);
            this.weekdayTabControl.Multiline = true;
            this.weekdayTabControl.Name = "weekdayTabControl";
            this.weekdayTabControl.Padding = new System.Drawing.Point(5, 5);
            this.weekdayTabControl.SelectedIndex = 0;
            this.weekdayTabControl.Size = new System.Drawing.Size(816, 386);
            this.weekdayTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.weekdayTabControl.TabIndex = 0;
            this.weekdayTabControl.Tag = "";
            this.weekdayTabControl.SelectedIndexChanged += new System.EventHandler(this.weekdayTabControl_SelectedIndexChanged);
            // 
            // mondayTabPage
            // 
            this.mondayTabPage.Controls.Add(this.weekdayScheduleControl);
            this.mondayTabPage.Location = new System.Drawing.Point(4, 26);
            this.mondayTabPage.Name = "mondayTabPage";
            this.mondayTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mondayTabPage.Size = new System.Drawing.Size(808, 356);
            this.mondayTabPage.TabIndex = 0;
            this.mondayTabPage.Tag = "1";
            this.mondayTabPage.Text = "Понедельник";
            this.mondayTabPage.UseVisualStyleBackColor = true;
            // 
            // weekdayScheduleControl
            // 
            this.weekdayScheduleControl.Location = new System.Drawing.Point(5, 5);
            this.weekdayScheduleControl.Name = "weekdayScheduleControl";
            this.weekdayScheduleControl.Schedule = null;
            this.weekdayScheduleControl.Size = new System.Drawing.Size(790, 310);
            this.weekdayScheduleControl.TabIndex = 0;
            // 
            // tuesdayTabPage
            // 
            this.tuesdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.tuesdayTabPage.Name = "tuesdayTabPage";
            this.tuesdayTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tuesdayTabPage.Size = new System.Drawing.Size(808, 356);
            this.tuesdayTabPage.TabIndex = 0;
            this.tuesdayTabPage.Tag = "2";
            this.tuesdayTabPage.Text = "Вторник";
            this.tuesdayTabPage.UseVisualStyleBackColor = true;
            // 
            // wednesdayTabPage
            // 
            this.wednesdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.wednesdayTabPage.Name = "wednesdayTabPage";
            this.wednesdayTabPage.Size = new System.Drawing.Size(808, 356);
            this.wednesdayTabPage.TabIndex = 0;
            this.wednesdayTabPage.Tag = "3";
            this.wednesdayTabPage.Text = "Среда";
            this.wednesdayTabPage.UseVisualStyleBackColor = true;
            // 
            // thursdayTabPage
            // 
            this.thursdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.thursdayTabPage.Name = "thursdayTabPage";
            this.thursdayTabPage.Size = new System.Drawing.Size(808, 356);
            this.thursdayTabPage.TabIndex = 0;
            this.thursdayTabPage.Tag = "4";
            this.thursdayTabPage.Text = "Четверг";
            this.thursdayTabPage.UseVisualStyleBackColor = true;
            // 
            // fridayTabPage
            // 
            this.fridayTabPage.Location = new System.Drawing.Point(4, 26);
            this.fridayTabPage.Name = "fridayTabPage";
            this.fridayTabPage.Size = new System.Drawing.Size(808, 356);
            this.fridayTabPage.TabIndex = 0;
            this.fridayTabPage.Tag = "5";
            this.fridayTabPage.Text = "Пятница";
            this.fridayTabPage.UseVisualStyleBackColor = true;
            // 
            // saturdayTabPage
            // 
            this.saturdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.saturdayTabPage.Name = "saturdayTabPage";
            this.saturdayTabPage.Size = new System.Drawing.Size(808, 356);
            this.saturdayTabPage.TabIndex = 0;
            this.saturdayTabPage.Tag = "6";
            this.saturdayTabPage.Text = "Суббота";
            this.saturdayTabPage.UseVisualStyleBackColor = true;
            // 
            // sundayTabPage
            // 
            this.sundayTabPage.Location = new System.Drawing.Point(4, 26);
            this.sundayTabPage.Name = "sundayTabPage";
            this.sundayTabPage.Size = new System.Drawing.Size(808, 356);
            this.sundayTabPage.TabIndex = 0;
            this.sundayTabPage.Tag = "0";
            this.sundayTabPage.Text = "Воскресенье";
            this.sundayTabPage.UseVisualStyleBackColor = true;
            // 
            // scheduleTabControl
            // 
            this.scheduleTabControl.Controls.Add(this.weekdayTabPage);
            this.scheduleTabControl.Controls.Add(this.exceptionTabPage);
            this.scheduleTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scheduleTabControl.Location = new System.Drawing.Point(5, 5);
            this.scheduleTabControl.Margin = new System.Windows.Forms.Padding(5);
            this.scheduleTabControl.Multiline = true;
            this.scheduleTabControl.Name = "scheduleTabControl";
            this.scheduleTabControl.Padding = new System.Drawing.Point(5, 5);
            this.scheduleTabControl.SelectedIndex = 0;
            this.scheduleTabControl.Size = new System.Drawing.Size(834, 426);
            this.scheduleTabControl.TabIndex = 0;
            // 
            // DefaultScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 436);
            this.Controls.Add(this.scheduleTabControl);
            this.MinimumSize = new System.Drawing.Size(860, 470);
            this.Name = "DefaultScheduleForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Общее расписание";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefaultScheduleForm_FormClosing);
            this.Load += new System.EventHandler(this.DefaultScheduleForm_Load);
            this.exceptionTabPage.ResumeLayout(false);
            this.exceptionTabPage.PerformLayout();
            this.weekdayTabPage.ResumeLayout(false);
            this.weekdayTabControl.ResumeLayout(false);
            this.mondayTabPage.ResumeLayout(false);
            this.scheduleTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage exceptionTabPage;
        private System.Windows.Forms.DateTimePicker exceptionScheduleDatePicker;
        private System.Windows.Forms.CheckBox exceptionScheduleCheckBox;
        private System.Windows.Forms.TabPage weekdayTabPage;
        private System.Windows.Forms.TabControl scheduleTabControl;
        private System.Windows.Forms.TabControl weekdayTabControl;
        private System.Windows.Forms.TabPage mondayTabPage;
        private System.Windows.Forms.TabPage tuesdayTabPage;
        private System.Windows.Forms.TabPage wednesdayTabPage;
        private System.Windows.Forms.TabPage thursdayTabPage;
        private System.Windows.Forms.TabPage fridayTabPage;
        private System.Windows.Forms.TabPage saturdayTabPage;
        private System.Windows.Forms.TabPage sundayTabPage;
        private global::Queue.Administrator.ScheduleControl weekdayScheduleControl;
        private global::Queue.Administrator.ScheduleControl exceptionScheduleControl;
    }
}