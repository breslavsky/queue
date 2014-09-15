namespace Queue.Metric
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.isValidateCheckBox = new System.Windows.Forms.CheckBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.iterationsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.databaseGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Controls.Add(this.isValidateCheckBox);
            this.databaseGroupBox.Controls.Add(this.connectButton);
            this.databaseGroupBox.Location = new System.Drawing.Point(10, 10);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Size = new System.Drawing.Size(245, 60);
            this.databaseGroupBox.TabIndex = 2;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "База данных";
            // 
            // isValidateCheckBox
            // 
            this.isValidateCheckBox.AutoSize = true;
            this.isValidateCheckBox.Checked = true;
            this.isValidateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isValidateCheckBox.Location = new System.Drawing.Point(125, 30);
            this.isValidateCheckBox.Name = "isValidateCheckBox";
            this.isValidateCheckBox.Size = new System.Drawing.Size(114, 17);
            this.isValidateCheckBox.TabIndex = 0;
            this.isValidateCheckBox.Text = "Проверить схему";
            this.isValidateCheckBox.UseVisualStyleBackColor = true;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(10, 25);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(105, 25);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Подключить";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iterationsLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 75);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(264, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // iterationsLabel
            // 
            this.iterationsLabel.Name = "iterationsLabel";
            this.iterationsLabel.Size = new System.Drawing.Size(12, 17);
            this.iterationsLabel.Text = "-";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 97);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.databaseGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Метрики";
            this.databaseGroupBox.ResumeLayout(false);
            this.databaseGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox databaseGroupBox;
        private System.Windows.Forms.CheckBox isValidateCheckBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel iterationsLabel;

    }
}

