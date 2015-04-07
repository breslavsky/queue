namespace Queue.Administrator
{
    partial class EditOperatorInterruptionForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.dayOfWeekLabel = new System.Windows.Forms.Label();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.finishTimeLabel = new System.Windows.Forms.Label();
            this.operatorControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.startTimePicker = new Queue.UI.WinForms.TimePicker();
            this.finishTimePicker = new Queue.UI.WinForms.TimePicker();
            this.dayOfWeekControl = new Queue.UI.WinForms.EnumItemControl();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(220, 115);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // operatorLabel
            // 
            this.operatorLabel.Location = new System.Drawing.Point(5, 10);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(135, 25);
            this.operatorLabel.TabIndex = 6;
            this.operatorLabel.Text = "Оператор";
            this.operatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dayOfWeekLabel
            // 
            this.dayOfWeekLabel.Location = new System.Drawing.Point(5, 35);
            this.dayOfWeekLabel.Name = "dayOfWeekLabel";
            this.dayOfWeekLabel.Size = new System.Drawing.Size(135, 25);
            this.dayOfWeekLabel.TabIndex = 7;
            this.dayOfWeekLabel.Text = "День недели";
            this.dayOfWeekLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.Location = new System.Drawing.Point(5, 60);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(135, 25);
            this.startTimeLabel.TabIndex = 8;
            this.startTimeLabel.Text = "Время начала";
            this.startTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // finishTimeLabel
            // 
            this.finishTimeLabel.Location = new System.Drawing.Point(5, 85);
            this.finishTimeLabel.Name = "finishTimeLabel";
            this.finishTimeLabel.Size = new System.Drawing.Size(135, 25);
            this.finishTimeLabel.TabIndex = 9;
            this.finishTimeLabel.Text = "Время окончания";
            this.finishTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // operatorControl
            // 
            this.operatorControl.Location = new System.Drawing.Point(140, 15);
            this.operatorControl.Name = "operatorControl";
            this.operatorControl.Size = new System.Drawing.Size(154, 21);
            this.operatorControl.TabIndex = 10;
            this.operatorControl.UseResetButton = false;
            this.operatorControl.Leave += new System.EventHandler(this.operatorControl_Leave);
            // 
            // startTimePicker
            // 
            this.startTimePicker.Location = new System.Drawing.Point(140, 65);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Size = new System.Drawing.Size(35, 20);
            this.startTimePicker.TabIndex = 11;
            this.startTimePicker.Value = System.TimeSpan.Parse("00:00:00");
            this.startTimePicker.Leave += new System.EventHandler(this.startTimePicker_Leave);
            // 
            // finishTimePicker
            // 
            this.finishTimePicker.Location = new System.Drawing.Point(140, 90);
            this.finishTimePicker.Name = "finishTimePicker";
            this.finishTimePicker.Size = new System.Drawing.Size(35, 20);
            this.finishTimePicker.TabIndex = 12;
            this.finishTimePicker.Value = System.TimeSpan.Parse("00:00:00");
            this.finishTimePicker.Leave += new System.EventHandler(this.finishTimePicker_Leave);
            // 
            // dayOfWeekControl
            // 
            this.dayOfWeekControl.Location = new System.Drawing.Point(140, 40);
            this.dayOfWeekControl.Name = "dayOfWeekControl";
            this.dayOfWeekControl.Size = new System.Drawing.Size(150, 21);
            this.dayOfWeekControl.TabIndex = 13;
            this.dayOfWeekControl.Leave += new System.EventHandler(this.dayOfWeekControl_Leave);
            // 
            // EditOperatorInterruptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 146);
            this.Controls.Add(this.dayOfWeekControl);
            this.Controls.Add(this.finishTimePicker);
            this.Controls.Add(this.startTimePicker);
            this.Controls.Add(this.operatorControl);
            this.Controls.Add(this.finishTimeLabel);
            this.Controls.Add(this.startTimeLabel);
            this.Controls.Add(this.dayOfWeekLabel);
            this.Controls.Add(this.operatorLabel);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(100, 130);
            this.Name = "EditOperatorInterruptionForm";
            this.Text = "Редактирование перерыва оператора";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditAdditionalServiceForm_FormClosed);
            this.Load += new System.EventHandler(this.EditOperatorInterruptionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.Label dayOfWeekLabel;
        private System.Windows.Forms.Label startTimeLabel;
        private System.Windows.Forms.Label finishTimeLabel;
        private UI.WinForms.IdentifiedEntityControl operatorControl;
        private UI.WinForms.TimePicker startTimePicker;
        private UI.WinForms.TimePicker finishTimePicker;
        private UI.WinForms.EnumItemControl dayOfWeekControl;
    }
}