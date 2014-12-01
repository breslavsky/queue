namespace Queue.UI.WinForms
{
    partial class EditServiceRenderingForm
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
            this.operatorComboBox = new System.Windows.Forms.ComboBox();
            this.serviceStepСomboBox = new System.Windows.Forms.ComboBox();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.serviceStepLabel = new System.Windows.Forms.Label();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityUpDown = new System.Windows.Forms.NumericUpDown();
            this.saveButton = new System.Windows.Forms.Button();
            this.modeСomboBox = new System.Windows.Forms.ComboBox();
            this.mode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.priorityUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // operatorComboBox
            // 
            this.operatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operatorComboBox.FormattingEnabled = true;
            this.operatorComboBox.Location = new System.Drawing.Point(110, 10);
            this.operatorComboBox.Name = "operatorComboBox";
            this.operatorComboBox.Size = new System.Drawing.Size(140, 21);
            this.operatorComboBox.TabIndex = 0;
            this.operatorComboBox.Leave += new System.EventHandler(this.operatorComboBox_Leave);
            // 
            // serviceStepСomboBox
            // 
            this.serviceStepСomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serviceStepСomboBox.FormattingEnabled = true;
            this.serviceStepСomboBox.Location = new System.Drawing.Point(110, 35);
            this.serviceStepСomboBox.Name = "serviceStepСomboBox";
            this.serviceStepСomboBox.Size = new System.Drawing.Size(140, 21);
            this.serviceStepСomboBox.TabIndex = 1;
            this.serviceStepСomboBox.Leave += new System.EventHandler(this.serviceStepСomboBox_Leave);
            // 
            // operatorLabel
            // 
            this.operatorLabel.Location = new System.Drawing.Point(10, 10);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(95, 20);
            this.operatorLabel.TabIndex = 5;
            this.operatorLabel.Text = "Оператор";
            this.operatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceStepLabel
            // 
            this.serviceStepLabel.Location = new System.Drawing.Point(10, 35);
            this.serviceStepLabel.Name = "serviceStepLabel";
            this.serviceStepLabel.Size = new System.Drawing.Size(95, 20);
            this.serviceStepLabel.TabIndex = 8;
            this.serviceStepLabel.Text = "Этап услуги";
            this.serviceStepLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(10, 85);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(95, 20);
            this.priorityLabel.TabIndex = 10;
            this.priorityLabel.Text = "Приоритет";
            this.priorityLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // priorityUpDown
            // 
            this.priorityUpDown.Location = new System.Drawing.Point(110, 85);
            this.priorityUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.priorityUpDown.Name = "priorityUpDown";
            this.priorityUpDown.Size = new System.Drawing.Size(60, 20);
            this.priorityUpDown.TabIndex = 14;
            this.priorityUpDown.Leave += new System.EventHandler(this.priorityUpDown_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(175, 110);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 15;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // modeСomboBox
            // 
            this.modeСomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeСomboBox.FormattingEnabled = true;
            this.modeСomboBox.Location = new System.Drawing.Point(110, 60);
            this.modeСomboBox.Name = "modeСomboBox";
            this.modeСomboBox.Size = new System.Drawing.Size(110, 21);
            this.modeСomboBox.TabIndex = 16;
            this.modeСomboBox.Leave += new System.EventHandler(this.modeСomboBox_Leave);
            // 
            // mode
            // 
            this.mode.Location = new System.Drawing.Point(10, 60);
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(95, 20);
            this.mode.TabIndex = 17;
            this.mode.Text = "Режим";
            this.mode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // EditServiceRenderingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 141);
            this.Controls.Add(this.mode);
            this.Controls.Add(this.modeСomboBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.priorityUpDown);
            this.Controls.Add(this.priorityLabel);
            this.Controls.Add(this.serviceStepLabel);
            this.Controls.Add(this.operatorLabel);
            this.Controls.Add(this.serviceStepСomboBox);
            this.Controls.Add(this.operatorComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditServiceRenderingForm";
            this.Text = "Изменение обслуживания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditServiceRenderingForm_FormClosing);
            this.Load += new System.EventHandler(this.EditServiceRenderingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.priorityUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox operatorComboBox;
        private System.Windows.Forms.ComboBox serviceStepСomboBox;
        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.Label serviceStepLabel;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.NumericUpDown priorityUpDown;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ComboBox modeСomboBox;
        private System.Windows.Forms.Label mode;
    }
}