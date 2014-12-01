namespace Queue.UI.WinForms
{
    partial class TerminalConfigControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.terminallayoutGroupBox = new System.Windows.Forms.GroupBox();
            this.columnsUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsUpDown = new System.Windows.Forms.NumericUpDown();
            this.terminalRowsLabel = new System.Windows.Forms.Label();
            this.terminalColumnsLabel = new System.Windows.Forms.Label();
            this.PINUpDown = new System.Windows.Forms.NumericUpDown();
            this.saveButton = new System.Windows.Forms.Button();
            this.terminalPINLabel = new System.Windows.Forms.Label();
            this.currentDayRecordingCheckBox = new System.Windows.Forms.CheckBox();
            this.terminallayoutGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PINUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // terminallayoutGroupBox
            // 
            this.terminallayoutGroupBox.Controls.Add(this.columnsUpDown);
            this.terminallayoutGroupBox.Controls.Add(this.rowsUpDown);
            this.terminallayoutGroupBox.Controls.Add(this.terminalRowsLabel);
            this.terminallayoutGroupBox.Controls.Add(this.terminalColumnsLabel);
            this.terminallayoutGroupBox.Location = new System.Drawing.Point(10, 35);
            this.terminallayoutGroupBox.Name = "terminallayoutGroupBox";
            this.terminallayoutGroupBox.Size = new System.Drawing.Size(230, 55);
            this.terminallayoutGroupBox.TabIndex = 10;
            this.terminallayoutGroupBox.TabStop = false;
            this.terminallayoutGroupBox.Text = "Расположение услуг";
            // 
            // ColumnsUpDown
            // 
            this.columnsUpDown.Location = new System.Drawing.Point(65, 20);
            this.columnsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.columnsUpDown.Name = "ColumnsUpDown";
            this.columnsUpDown.Size = new System.Drawing.Size(50, 20);
            this.columnsUpDown.TabIndex = 1;
            this.columnsUpDown.Leave += new System.EventHandler(this.columnsUpDown_Leave);
            // 
            // RowsUpDown
            // 
            this.rowsUpDown.Location = new System.Drawing.Point(165, 20);
            this.rowsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.rowsUpDown.Name = "RowsUpDown";
            this.rowsUpDown.Size = new System.Drawing.Size(50, 20);
            this.rowsUpDown.TabIndex = 3;
            this.rowsUpDown.Leave += new System.EventHandler(this.rowsUpDown_Leave);
            // 
            // terminalRowsLabel
            // 
            this.terminalRowsLabel.AutoSize = true;
            this.terminalRowsLabel.Location = new System.Drawing.Point(125, 25);
            this.terminalRowsLabel.Name = "terminalRowsLabel";
            this.terminalRowsLabel.Size = new System.Drawing.Size(37, 13);
            this.terminalRowsLabel.TabIndex = 4;
            this.terminalRowsLabel.Text = "Строк";
            // 
            // terminalColumnsLabel
            // 
            this.terminalColumnsLabel.AutoSize = true;
            this.terminalColumnsLabel.Location = new System.Drawing.Point(12, 25);
            this.terminalColumnsLabel.Name = "terminalColumnsLabel";
            this.terminalColumnsLabel.Size = new System.Drawing.Size(50, 13);
            this.terminalColumnsLabel.TabIndex = 2;
            this.terminalColumnsLabel.Text = "Колонок";
            // 
            // PINUpDown
            // 
            this.PINUpDown.Location = new System.Drawing.Point(65, 5);
            this.PINUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.PINUpDown.Name = "PINUpDown";
            this.PINUpDown.Size = new System.Drawing.Size(70, 20);
            this.PINUpDown.TabIndex = 6;
            this.PINUpDown.Leave += new System.EventHandler(this.PINUpDown_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(165, 100);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Сохранить";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // terminalPINLabel
            // 
            this.terminalPINLabel.AutoSize = true;
            this.terminalPINLabel.Location = new System.Drawing.Point(7, 10);
            this.terminalPINLabel.Name = "terminalPINLabel";
            this.terminalPINLabel.Size = new System.Drawing.Size(46, 13);
            this.terminalPINLabel.TabIndex = 8;
            this.terminalPINLabel.Text = "PIN-код";
            // 
            // CurrentDayRecordingCheckBox
            // 
            this.currentDayRecordingCheckBox.AutoSize = true;
            this.currentDayRecordingCheckBox.Location = new System.Drawing.Point(10, 105);
            this.currentDayRecordingCheckBox.Name = "CurrentDayRecordingCheckBox";
            this.currentDayRecordingCheckBox.Size = new System.Drawing.Size(151, 17);
            this.currentDayRecordingCheckBox.TabIndex = 9;
            this.currentDayRecordingCheckBox.Text = "Запись на текущий день";
            this.currentDayRecordingCheckBox.UseVisualStyleBackColor = true;
            this.currentDayRecordingCheckBox.Leave += new System.EventHandler(this.currentDayRecordingCheckBox_Leave);
            // 
            // TerminalConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.terminallayoutGroupBox);
            this.Controls.Add(this.PINUpDown);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.terminalPINLabel);
            this.Controls.Add(this.currentDayRecordingCheckBox);
            this.Name = "TerminalConfigControl";
            this.Size = new System.Drawing.Size(250, 140);
            this.terminallayoutGroupBox.ResumeLayout(false);
            this.terminallayoutGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PINUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox terminallayoutGroupBox;
        private System.Windows.Forms.NumericUpDown columnsUpDown;
        private System.Windows.Forms.NumericUpDown rowsUpDown;
        private System.Windows.Forms.Label terminalRowsLabel;
        private System.Windows.Forms.Label terminalColumnsLabel;
        private System.Windows.Forms.NumericUpDown PINUpDown;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label terminalPINLabel;
        private System.Windows.Forms.CheckBox currentDayRecordingCheckBox;
    }
}
